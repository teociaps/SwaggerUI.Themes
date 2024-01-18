using AspNetCore.SwaggerUI.Themes;
using NSwag.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

// TODO: add summaries
// TODO: check for SwaggerUI style
// TODO: check for ReDoc

public static class StyleNSwagBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerUi(this WebApplication app, Style style, Action<SwaggerUiSettings> configure = null)
    {
        var path = StyleProvider.StylePath + style.FileName;

        ImportSwaggerStyle(app, style, path);

        Action<SwaggerUiSettings> defaultSetupAction = s => s.CustomStylesheetPath = path;
        if (configure is not null)
        {
            defaultSetupAction = settings =>
            {
                defaultSetupAction(settings);
                configure(settings);
            };
        }

        return app.UseSwaggerUi(defaultSetupAction);
    }

    public static IApplicationBuilder UseReDoc(this WebApplication app, Style style, Action<ReDocSettings> configure = null)
    {
        var path = StyleProvider.StylePath + style.FileName;

        ImportSwaggerStyle(app, style, path);

        Action<ReDocSettings> defaultSetupAction = s => s.CustomStylesheetPath = path;
        if (configure is not null)
        {
            defaultSetupAction = settings =>
            {
                defaultSetupAction(settings);
                configure(settings);
            };
        }

        return app.UseReDoc(defaultSetupAction);
    }


    private static void ImportSwaggerStyle(WebApplication app, Style style, string fullStylePath)
    {
        string stylesheet = StyleProvider.GetResourceText(style.FileName);

        StyleProvider.AddGetEndpoint(app, fullStylePath, stylesheet, style.FormatText);
    }
}