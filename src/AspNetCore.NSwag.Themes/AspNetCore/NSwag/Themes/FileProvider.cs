using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace AspNetCore.NSwag.Themes;

internal static class FileProvider
{
    private static readonly string _prefix = typeof(FileProvider).Namespace;
    private static readonly string _stylesNamespace = _prefix + ".Styles.";
    private static readonly string _scriptsNamespace = _prefix + ".Scripts.";

    internal const string StylesPath = "/styles/";
    internal const string ScriptsPath = "/scripts/";

    internal static string GetResourceText(string fileName)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var resource = string.Concat(IsCssFile(fileName) ? _stylesNamespace : _scriptsNamespace, fileName);

        using var stream = currentAssembly.GetManifestResourceStream(resource)
            ?? throw new FileNotFoundException($"Can't find {fileName} resource.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
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