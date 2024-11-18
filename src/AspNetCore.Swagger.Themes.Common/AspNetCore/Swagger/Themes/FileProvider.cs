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

    internal static string GetResourceText(string fileName, Type styleType = null)
    {
        var assembly = styleType?.Assembly ?? Assembly.GetExecutingAssembly();
        var resourceNamespace = DetermineResourceNamespace(fileName, styleType);

        var resourceName = $"{resourceNamespace}.{fileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Can't find {fileName} resource in assembly {assembly.GetName().Name}.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    internal static string GetResourceText(string fileName, Assembly assembly, out string commonStyle, out bool loadModernJs)
    {
        if (!IsCssFile(fileName))
            throw new InvalidOperationException($"{fileName} is not a valid name for CSS files. It must ends with '.css'.");

        var resourceNamespaces = assembly.GetManifestResourceNames().Where(n => n.EndsWith(_CustomStylesNamespace + fileName, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (resourceNamespaces.Length != 1)
            throw new InvalidOperationException($"Can't find {fileName} or it appears more than one time in assembly {assembly.GetName().Name}.");

        var resourceName = resourceNamespaces[0];

        // Retrieve the common style based on the css filename (classic or modern) and check if JS needs to be loaded for modern styles
        // This is a shortcut to inherit common style without making a new style class
        commonStyle = RetrieveCommonStyleFromCustom(resourceName, out loadModernJs);

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

    private static bool IsCssFile(string fileName) => fileName.EndsWith(".css");

    private static string DetermineResourceNamespace(string fileName, Type styleType)
    {
        if (IsCssFile(fileName) && styleType is not null && styleType.BaseType != typeof(BaseStyle))
            return styleType.Namespace;

        return IsCssFile(fileName) ? _StylesNamespace : _ScriptsNamespace;
    }

    private static string RetrieveCommonStyleFromCustom(string resourceName, out bool loadModernJs)
    {
        string commonStyle = string.Empty;
        loadModernJs = false;

        int index = resourceName.IndexOf(_CustomStylesNamespace);
        if (index == -1)
            return commonStyle;

        string styleSuffix = resourceName[(index + _CustomStylesNamespace.Length)..];

        bool isClassicStyle = styleSuffix.StartsWith("classic.");
        bool isModernStyle = loadModernJs = styleSuffix.StartsWith("modern.");

        if (isClassicStyle)
            return GetResourceText("common.css");

        if (isModernStyle)
            return GetResourceText("modern.common.css");

        return commonStyle;
    }

    #endregion Private
}