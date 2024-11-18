using AspNetCore.Swagger.Themes;
using NSwag.AspNetCore;
using System.Reflection;
using System.Text;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class NSwagBuilderExtensions
{
    /// <summary>
    /// Registers the Swagger UI middleware with a specified style and optional settings setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="configureSettings">An optional action to configure Swagger UI settings.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        BaseStyle style,
        Action<SwaggerUiSettings> configureSettings = null)
    {
        ArgumentNullException.ThrowIfNull(style);

        return application.UseSwaggerUi(uiSettings =>
        {
            configureSettings?.Invoke(uiSettings);

            uiSettings.CustomInlineStyles = GetSwaggerStyleCss(style, uiSettings);

            if (style.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(uiSettings.AdditionalSettings))
                AddCustomJavascript(application, uiSettings);
        });
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS style and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="cssStyleContent">The CSS style to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="cssStyleContent"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        string cssStyleContent,
        Action<SwaggerUiSettings> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(cssStyleContent);

        setupAction += options => options.CustomInlineStyles = cssStyleContent;

        return application.UseSwaggerUi(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS style and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="assembly">The assembly where the embedded CSS file is situated.</param>
    /// <param name="cssFilename">The CSS style filename (e.g. "myCustomStyle.css").</param>
    /// <param name="configureSettings">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> or <paramref name="cssFilename"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        Assembly assembly,
        string cssFilename,
        Action<SwaggerUiSettings> configureSettings = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(cssFilename);

        var settings = new SwaggerUiSettings();
        configureSettings?.Invoke(settings);

        var stylesheet = FileProvider.GetResourceText(cssFilename, assembly, out var commonStyle, out var loadModernJs);

        if (!string.IsNullOrEmpty(commonStyle))
        {
            commonStyle = AdvancedOptions.Apply(commonStyle, settings.AdditionalSettings, MimeTypes.Text.Css);
            stylesheet = commonStyle + Environment.NewLine + stylesheet;

            if (loadModernJs && AdvancedOptions.AnyJsFeatureEnabled(settings.AdditionalSettings))
                configureSettings += settings => AddCustomJavascript(application, settings);
        }

        configureSettings += options => options.CustomInlineStyles = stylesheet;

        return application.UseSwaggerUi(configureSettings);
    }

    // TODO: add other extension methods from nswag?

    #region Private

    private static string GetSwaggerStyleCss(BaseStyle style, SwaggerUiSettings settings)
    {
        var sb = new StringBuilder();

        string baseCss = FileProvider.GetResourceText(style.Common.FileName);
        baseCss = AdvancedOptions.Apply(baseCss, settings.AdditionalSettings, MimeTypes.Text.Css);

        string styleCss = FileProvider.GetResourceText(style.FileName, style.GetType());

        sb.Append(baseCss);
        sb.Append('\n');
        sb.Append(styleCss);

        return sb.ToString();
    }

    private static string GetSwaggerStyleJavascriptPath(IApplicationBuilder application, SwaggerUiSettings settings)
    {
        const string JsFilename = "modern.js";
        string javascript = FileProvider.GetResourceText(JsFilename);
        javascript = AdvancedOptions.Apply(javascript, settings.AdditionalSettings, MimeTypes.Text.Javascript);

        const string FullPath = FileProvider.ScriptsPath + JsFilename;
        FileProvider.AddGetEndpoint(application, FullPath, javascript, MimeTypes.Text.Javascript);

        return FullPath;
    }

    private static void AddCustomJavascript(IApplicationBuilder application, SwaggerUiSettings settings)
        => settings.CustomJavaScriptPath = GetSwaggerStyleJavascriptPath(application, settings);

    #endregion Private
}