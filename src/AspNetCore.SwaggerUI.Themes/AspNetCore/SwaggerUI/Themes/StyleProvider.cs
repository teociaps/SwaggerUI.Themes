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

    internal static void AddGetEndpoint(WebApplication app, string fullStylePath, string styleText, string styleFormat)
    {
        app.MapGet(fullStylePath, (HttpContext context) =>
        {
            // TODO: change cache time?
            context.Response.Headers.CacheControl = "public, max-age=3600";
            context.Response.Headers.Expires = DateTime.UtcNow.AddDays(2).ToString("R");

            return Results.Content(styleText, $"text/{styleFormat}");
        })
        .ExcludeFromDescription();
    }
}