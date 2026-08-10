using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Handles file I/O and HTTP endpoint registration for themes.
/// </summary>
internal static class FileProvider
{
    private const string _Prefix = "AspNetCore.Swagger.Themes";
    private const string _StylesNamespace = _Prefix + ".Styles";
    private const string _ScriptsNamespace = _Prefix + ".Scripts";
    private const string _CustomStylesNamespace = "SwaggerThemes.";

    internal const string StylesPath = "/styles/";
    internal const string ScriptsPath = "/scripts/";
    internal const string ThemeMetadataPath = "/themes/metadata.json";
    internal const string JsFilename = "ui.min.js";

    // Track registered endpoints to prevent duplicates and act as the shared dispatch table for the single middleware
    private static readonly ConcurrentDictionary<string, (string Content, string ContentType)> s_registeredEndpoints = new(StringComparer.OrdinalIgnoreCase);

    // Tracks which IApplicationBuilder instances already have the dispatch middleware registered
    private static readonly ConditionalWeakTable<IApplicationBuilder, object> s_dispatchRegisteredApps = new();

    private static FrozenSet<string> s_frozenEndpoints;

    // Predefined theme names (compile-time constant)
    private static readonly FrozenSet<string> s_predefinedThemeNames = new[]
    {
        nameof(Theme.Dark), nameof(Theme.Light), nameof(Theme.Forest), nameof(Theme.DeepSea), nameof(Theme.Desert), nameof(Theme.Futuristic)
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    private static readonly JsonSerializerOptions s_cachedJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    /// <summary>
    /// Gets resource text from embedded resources.
    /// </summary>
    internal static string GetResourceText(string fileName, Type themeType = null)
    {
        var assembly = themeType?.Assembly ?? Assembly.GetExecutingAssembly();
        var resourceNamespace = DetermineResourceNamespace(fileName, themeType);
        var resourceName = $"{resourceNamespace}.{fileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Can't find {fileName} resource in assembly {assembly.GetName().Name}.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Gets resource text from assembly with common style detection.
    /// </summary>
    internal static string GetResourceText(string fileName, Assembly assembly, out string commonStyle, out bool loadJs)
    {
        if (!IsCssFile(fileName))
            throw new InvalidOperationException($"{fileName} is not a valid CSS file. Must end with '.css' or '.min.css'.");

        // Get all SwaggerThemes resources
        var swaggerThemesResources = assembly.GetManifestResourceNames()
            .Where(n => n.Contains(_CustomStylesNamespace, StringComparison.OrdinalIgnoreCase))
            .Where(resourceName =>
            {
                // Must end with ".{fileName}" to ensure exact match
                if (!resourceName.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase))
                    return false;

                // Special case: Exclude "standalone.{fileName}" when searching for "{fileName}"
                // This prevents "{fileName}.css" from matching "standalone.{fileName}.css"
                var idx = resourceName.LastIndexOf(_CustomStylesNamespace, StringComparison.OrdinalIgnoreCase);
                if (idx != -1)
                {
                    var afterNamespace = resourceName[(idx + _CustomStylesNamespace.Length)..];

                    if (afterNamespace.StartsWith("standalone.", StringComparison.OrdinalIgnoreCase) &&
                        !fileName.StartsWith("standalone.", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            })
            .ToList();

        if (swaggerThemesResources.Count == 0)
            throw new FileNotFoundException($"Can't find {fileName} in any {_CustomStylesNamespace}* namespace in {assembly.GetName().Name}.");

        if (swaggerThemesResources.Count > 1)
        {
            var matchingPaths = string.Join(", ", swaggerThemesResources);
            throw new InvalidOperationException(
                $"Found {fileName} in multiple locations in {assembly.GetName().Name}: {matchingPaths}. " +
                "Ensure the file name is unique across all theme folders.");
        }

        var resourceName = swaggerThemesResources[0];
        commonStyle = RetrieveCommonStyleFromCustom(resourceName, out loadJs);

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Can't find {fileName} in {assembly.GetName().Name}.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Registers a GET endpoint for serving static content.
    /// </summary>
    internal static void AddGetEndpoint(IApplicationBuilder app, string path, string content, string contentType = MimeTypes.Text.Css)
    {
        if (!s_registeredEndpoints.TryAdd(path, (content, contentType)))
            return;

        // Register the single dispatch middleware only the first time this app instance is seen.
        // Every subsequent call just adds an entry to the shared dispatch table above.
        s_dispatchRegisteredApps.GetValue(app, static application =>
        {
            application.Use(async (context, next) =>
            {
                if (s_registeredEndpoints.TryGetValue(context.Request.Path.Value ?? string.Empty, out var endpoint))
                {
                    SetCacheHeaders(context);
                    context.Response.ContentType = endpoint.ContentType;
                    await context.Response.WriteAsync(endpoint.Content);
                }
                else
                {
                    await next();
                }
            });

            return new object();
        });

        static void SetCacheHeaders(HttpContext context)
        {
            context.Response.Headers.CacheControl = "max-age=3600";
            context.Response.Headers.Expires = DateTime.UtcNow.AddHours(1).ToString("R");
        }
    }

    /// <summary>
    /// Exposes theme metadata as JSON endpoint.
    /// </summary>
    internal static void ExposeThemeMetadata(
        IApplicationBuilder app,
        IEnumerable<RegisteredTheme> themes,
        string currentThemeName,
        string displayFormat)
    {
        if (s_registeredEndpoints.ContainsKey(ThemeMetadataPath))
            return;

        var themeList = themes.Select(rt => new
        {
            name = rt.Name,
            displayName = rt.Name,
            cssPath = rt.CssPath,
            isStandalone = rt.IsStandalone
        }).ToList();

        // Ensure current theme is first
        var current = themeList.FirstOrDefault(t => t.name.Equals(currentThemeName, StringComparison.OrdinalIgnoreCase));
        if (current is not null)
        {
            themeList.Remove(current);
            themeList.Insert(0, current);
        }

        var response = new
        {
            themes = themeList,
            config = new
            {
                displayFormat,
                currentTheme = currentThemeName
            }
        };

        var json = JsonSerializer.Serialize(response, s_cachedJsonOptions);

        AddGetEndpoint(app, ThemeMetadataPath, json, MimeTypes.Application.Json);
    }

    /// <summary>
    /// Freezes collections after startup for better read performance.
    /// </summary>
    internal static void FreezeCollections() =>
        s_frozenEndpoints ??= s_registeredEndpoints.Keys.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if a theme name is predefined.
    /// </summary>
    internal static bool IsPredefinedTheme(string themeName) =>
        s_predefinedThemeNames.Contains(themeName);

    private static bool IsCssFile(string fileName) =>
        fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
        fileName.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase);

    private static string DetermineResourceNamespace(string fileName, Type themeType)
    {
        if (IsCssFile(fileName) && themeType is not null && themeType.BaseType != typeof(BaseTheme))
            return themeType.Namespace!;

        return IsCssFile(fileName) ? _StylesNamespace : _ScriptsNamespace;
    }

    private static string RetrieveCommonStyleFromCustom(string resourceName, out bool loadJs)
    {
        loadJs = false;

        if (!resourceName.Contains(_CustomStylesNamespace))
            return string.Empty;

        var fileName = resourceName[(resourceName.IndexOf(_CustomStylesNamespace) + _CustomStylesNamespace.Length)..];

        if (fileName.Contains("standalone", StringComparison.OrdinalIgnoreCase))
            return string.Empty;

        loadJs = true;
        return GetResourceText("common.min.css");
    }
}