using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace AspNetCore.SwaggerUI.Themes;

internal static class StyleProvider
{
    private static readonly string _stylesNamespace = string.Concat(typeof(StyleProvider).Namespace, ".Styles.");

    // TODO: internal const string CommonStyleFile = "common.css";

    internal static string GetResourceText(string fileName)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var resource = string.Concat(_stylesNamespace, fileName);

        using var stream = currentAssembly.GetManifestResourceStream(resource)
            ?? throw new FileNotFoundException("Can't find theme resource.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    internal static void AddGetEndpoint(WebApplication app, string cssPath, string styleText)
    {
        app.MapGet(cssPath, (HttpContext context) =>
        {
            context.Response.Headers.CacheControl = "public, max-age=3600";
            context.Response.Headers.Expires = DateTime.UtcNow.AddDays(2).ToString("R");

            return Results.Content(styleText, "text/css"); // TODO: add different formats in the future
        })
        .ExcludeFromDescription();
    }
}