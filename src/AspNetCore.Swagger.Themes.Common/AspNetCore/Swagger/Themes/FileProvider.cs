using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace AspNetCore.Swagger.Themes;

internal static class FileProvider
{
    private const string _Prefix = "AspNetCore.Swagger.Themes";
    private const string _StylesNamespace = _Prefix + ".Styles";
    private const string _ScriptsNamespace = _Prefix + ".Scripts";
    private const string _CustomStylesNamespace = "SwaggerThemes.";

    internal const string StylesPath = "/styles/";
    internal const string ScriptsPath = "/scripts/";

    internal const string JsFilename = "ui.min.js";

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

    internal static string GetResourceText(string fileName, Assembly assembly, out string commonStyle, out bool loadJs)
    {
        if (!IsCssFile(fileName))
            throw new InvalidOperationException($"{fileName} is not a valid name for CSS files. It must end with '.css' or '.min.css'.");

        var resourceNamespaces = assembly.GetManifestResourceNames()
            .Where(n => n.EndsWith(_CustomStylesNamespace + fileName, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (resourceNamespaces.Length != 1)
            throw new InvalidOperationException($"Can't find {fileName} or it appears more than one time in assembly {assembly.GetName().Name}.");

        var resourceName = resourceNamespaces[0];

        // Retrieve the common theme and determine if JS needs to be loaded
        commonStyle = RetrieveCommonStyleFromCustom(resourceName, out loadJs);

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Can't find {fileName} resource in assembly {assembly.GetName().Name}.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    internal static void AddGetEndpoint(IApplicationBuilder app, string path, string content, string contentType = MimeTypes.Text.Css)
    {
        if (app is WebApplication webApp)
        {
            webApp.MapGet(path, (HttpContext context) =>
            {
                SetHeaders(context);

                return Results.Content(content, contentType);
            })
            .ExcludeFromDescription();
        }
        else
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    SetHeaders(context);

                    context.Response.ContentType = contentType;
                    await context.Response.WriteAsync(content);
                }
                else
                {
                    await next();
                }
            });
        }

        static void SetHeaders(HttpContext context)
        {
            context.Response.Headers.CacheControl = "max-age=3600";
            context.Response.Headers.Expires = DateTime.UtcNow.AddHours(1).ToString("R");
        }
    }

    #region Private

    private static bool IsCssFile(string fileName) =>
        fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
        fileName.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase);

    private static string DetermineResourceNamespace(string fileName, Type themeType)
    {
        if (IsCssFile(fileName) && themeType is not null && themeType.BaseType != typeof(BaseTheme))
            return themeType.Namespace;

        return IsCssFile(fileName) ? _StylesNamespace : _ScriptsNamespace;
    }

    private static string RetrieveCommonStyleFromCustom(string resourceName, out bool loadJs)
    {
        loadJs = false;

        if (!resourceName.Contains(_CustomStylesNamespace))
            return string.Empty;

        // Extract filename from resource name
        int index = resourceName.IndexOf(_CustomStylesNamespace);
        string fileName = resourceName[(index + _CustomStylesNamespace.Length)..];

        // If filename contains "standalone", don't load anything (fully independent)
        if (fileName.Contains("standalone", StringComparison.OrdinalIgnoreCase))
            return string.Empty;

        // Otherwise, load both common theme and JS
        loadJs = true;
        return GetResourceText("common.min.css");
    }

    #endregion Private
}