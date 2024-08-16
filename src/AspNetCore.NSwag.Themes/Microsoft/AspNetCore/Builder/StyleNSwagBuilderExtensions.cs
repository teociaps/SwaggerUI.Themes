using AspNetCore.Swagger.Themes;
using NSwag.AspNetCore;
using System.Text;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class StyleNSwagBuilderExtensions
{
    /// <summary>
    /// Registers the Swagger UI middleware with a specified style and optional settings setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="configureSettings">An optional action to configure Swagger UI settings.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        BaseStyle style,
        Action<SwaggerUiSettings> configureSettings = null)
    {
        ArgumentNullException.ThrowIfNull(style);

        Action<SwaggerUiSettings> swaggerUiSettingsAction = options =>
        {
            options.CustomInlineStyles = GetSwaggerStyleCss(style);

            if (style.IsModern)
                options.CustomJavaScriptPath = GetSwaggerStyleJavascriptPath(application);
        };

        swaggerUiSettingsAction += configureSettings;

        return application.UseSwaggerUi(swaggerUiSettingsAction);
    }

    // TODO: add other extension methods from nswag?

    #region Private

    private static string GetSwaggerStyleCss(BaseStyle style)
    {
        var sb = new StringBuilder();

        string baseCss = FileProvider.GetResourceText(style.Common.FileName);
        string styleCss = FileProvider.GetResourceText(style.FileName, style.GetType());

        sb.Append(baseCss);
        sb.Append('\n');
        sb.Append(styleCss);

        return sb.ToString();
    }

    private static string GetSwaggerStyleJavascriptPath(IApplicationBuilder app)
    {
        const string JsFilename = "modern.js";
        string javascript = FileProvider.GetResourceText(JsFilename);
        const string FullPath = FileProvider.ScriptsPath + JsFilename;

        FileProvider.AddGetEndpoint(app, FullPath, javascript, MimeTypes.Text.Javascript);

        return FullPath;
    }

    #endregion Private
}