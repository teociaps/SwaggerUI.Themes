using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace AspNetCore.Swagger.Themes;

internal static class FileProvider
{
    private const string _Prefix = "AspNetCore.Swagger.Themes";
    private const string _StylesNamespace = _Prefix + ".Styles";
    private const string _ScriptsNamespace = _Prefix + ".Scripts";

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

    private static string DetermineResourceNamespace(string fileName, Type styleType)
    {
        if (IsCssFile(fileName) && styleType is not null && styleType.BaseType != typeof(BaseStyle))
        {
            return styleType.Namespace;
        }

        return IsCssFile(fileName) ? _StylesNamespace : _ScriptsNamespace;
    }


    internal static void AddGetEndpoint(WebApplication app, string path, string content, string contentType = MimeTypes.Text.Css)
    {
        app.MapGet(path, (HttpContext context) =>
        {
            context.Response.Headers.CacheControl = "max-age=3600";
            context.Response.Headers.Expires = DateTime.UtcNow.AddHours(1).ToString("R");

            return Results.Content(content, contentType);
        })
        .ExcludeFromDescription();
    }

    private static bool IsCssFile(string fileName) => fileName.EndsWith(".css");
}