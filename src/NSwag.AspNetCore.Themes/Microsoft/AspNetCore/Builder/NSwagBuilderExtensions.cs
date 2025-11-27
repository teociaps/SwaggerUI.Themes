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
    /// Registers the Swagger UI middleware with a specified theme and optional settings setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="theme">The theme to apply.</param>
    /// <param name="configureSettings">An optional action to configure Swagger UI settings.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="theme"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        BaseTheme theme,
        Action<SwaggerUiSettings> configureSettings = null)
    {
        ArgumentNullException.ThrowIfNull(theme);

        return application.UseSwaggerUi(uiSettings =>
        {
            configureSettings?.Invoke(uiSettings);

            uiSettings.CustomInlineStyles = GetSwaggerThemeCss(theme, uiSettings);

            if (theme.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(uiSettings.AdditionalSettings))
                AddCustomJavascript(application, uiSettings);
        });
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS theme and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="cssThemeContent">The CSS theme to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="cssThemeContent"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        string cssThemeContent,
        Action<SwaggerUiSettings> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(cssThemeContent);

        setupAction += options => options.CustomInlineStyles = cssThemeContent;

        return application.UseSwaggerUi(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS theme and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="assembly">The assembly where the embedded CSS file is situated.</param>
    /// <param name="cssFilename">The CSS theme filename (e.g. "myCustomTheme.css").</param>
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

        var theme = FileProvider.GetResourceText(cssFilename, assembly, out var commonTheme, out var loadJs);

        if (!string.IsNullOrEmpty(commonTheme))
        {
            commonTheme = AdvancedOptions.Apply(commonTheme, settings.AdditionalSettings, MimeTypes.Text.Css);
            theme = commonTheme + Environment.NewLine + theme;

            if (loadJs && AdvancedOptions.AnyJsFeatureEnabled(settings.AdditionalSettings))
                configureSettings += settings => AddCustomJavascript(application, settings);
        }

        configureSettings += options => options.CustomInlineStyles = theme;

        return application.UseSwaggerUi(configureSettings);
    }

    // TODO: add other extension methods from nswag?

    #region Private

    private static string GetSwaggerThemeCss(BaseTheme theme, SwaggerUiSettings settings)
    {
        var sb = new StringBuilder();

        string baseCss = FileProvider.GetResourceText(theme.Common.FileName);
        baseCss = AdvancedOptions.Apply(baseCss, settings.AdditionalSettings, MimeTypes.Text.Css);

        string themeCss = FileProvider.GetResourceText(theme.FileName, theme.GetType());

        sb.Append(baseCss);
        sb.Append('\n');
        sb.Append(themeCss);

        return sb.ToString();
    }

    private static string GetSwaggerThemeJavascriptPath(IApplicationBuilder application, SwaggerUiSettings settings)
    {
        string javascript = FileProvider.GetResourceText(FileProvider.JsFilename);
        javascript = AdvancedOptions.Apply(javascript, settings.AdditionalSettings, MimeTypes.Text.Javascript);

        const string FullPath = FileProvider.ScriptsPath + FileProvider.JsFilename;
        FileProvider.AddGetEndpoint(application, FullPath, javascript, MimeTypes.Text.Javascript);

        return FullPath;
    }

    private static void AddCustomJavascript(IApplicationBuilder application, SwaggerUiSettings settings)
        => settings.CustomJavaScriptPath = GetSwaggerThemeJavascriptPath(application, settings);

    #endregion Private
}