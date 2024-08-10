using AspNetCore.NSwag.Themes;
using NSwag.AspNetCore;
using System.Text;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class StyleNSwagBuilderExtensions
{
    /// <summary>
    /// Register the SwaggerUI middleware using the specified style. You can override the behavior
    /// by providing custom settings.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="configure">The SwaggerUI settings.</param>
    public static IApplicationBuilder UseSwaggerUi(this WebApplication app, BaseStyle style, Action<SwaggerUiSettings> configure = null)
    {
        Action<SwaggerUiSettings> swaggerUiSettingsAction = options =>
        {
            options.CustomInlineStyles = GetSwaggerStyleCss(style);

            if (style.IsModern)
                options.CustomJavaScriptPath = GetSwaggerStyleJavascriptPath(app);
        };

        swaggerUiSettingsAction += configure;

        return app.UseSwaggerUi(swaggerUiSettingsAction);
    }

    private static string GetSwaggerStyleCss(BaseStyle style)
    {
        var sb = new StringBuilder();

        string baseCss = FileProvider.GetResourceText(style.Common.FileName);
        string styleCss = FileProvider.GetResourceText(style.FileName);

        sb.Append(baseCss);
        sb.Append('\n');
        sb.Append(styleCss);

        return sb.ToString();
    }

    private static string GetSwaggerStyleJavascriptPath(WebApplication app)
    {
        const string JsFilename = "modern.js";
        string javascript = FileProvider.GetResourceText(JsFilename);
        const string FullPath = FileProvider.ScriptsPath + JsFilename;

        FileProvider.AddGetEndpoint(app, FullPath, javascript, MimeTypes.Text.Javascript);

        return FullPath;
    }
}