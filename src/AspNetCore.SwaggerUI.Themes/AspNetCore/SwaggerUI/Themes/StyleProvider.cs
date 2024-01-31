using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace AspNetCore.SwaggerUI.Themes;

internal static class StyleProvider
{
    private static readonly string _stylesNamespace = string.Concat(typeof(StyleProvider).Namespace, ".Styles.");

    internal static readonly string StylePath = "/styles/";

    // TODO: internal const string CommonStyleFile = "common.css";

    internal static string GetResourceText(string fileName)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var resource = string.Concat(_stylesNamespace, fileName);

        using var stream = currentAssembly.GetManifestResourceStream(resource)
            ?? throw new FileNotFoundException($"Can't find {fileName} resource.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    internal static void AddGetEndpoint(WebApplication app, string fullStylePath, string styleContent, string styleFormat)
    {
        app.MapGet(fullStylePath, (HttpContext context) =>
        {
            context.Response.Headers.CacheControl = "max-age=3600";
            context.Response.Headers.Expires = DateTime.UtcNow.AddHours(1).ToString("R");

            return Results.Content(styleContent, $"text/{styleFormat}");
        })
        .ExcludeFromDescription();
    }
}