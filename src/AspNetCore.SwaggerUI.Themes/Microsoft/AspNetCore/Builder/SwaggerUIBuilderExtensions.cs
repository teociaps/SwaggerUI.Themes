using AspNetCore.Swagger.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class SwaggerUIBuilderExtensions
{
    /// <summary>
    /// Registers the Swagger UI middleware with a specified theme and optional configuration.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="theme">The theme to apply.</param>
    /// <param name="options">The Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="theme"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        BaseTheme theme,
        SwaggerUIOptions options)
    {
        ArgumentNullException.ThrowIfNull(theme);

        options ??= new SwaggerUIOptions();
        ConfigureSwaggerUIOptions(application, options, theme).Invoke(options);

        return application.UseSwaggerUI(options);
    }

    /// <summary>
    /// Registers the Swagger UI middleware with a specified theme and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="theme">The theme to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="theme"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        BaseTheme theme,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(theme);

        var options = new SwaggerUIOptions();
        setupAction?.Invoke(options);

        var optionsAction = ConfigureSwaggerUIOptions(application, options, theme);

        if (setupAction is not null)
            optionsAction += setupAction;

        return application.UseSwaggerUI(optionsAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS theme and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="cssThemeContent">The CSS theme to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="cssThemeContent"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        string cssThemeContent,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(cssThemeContent);

        const string CustomCssThemePath = $"{FileProvider.StylesPath}custom.css";
        FileProvider.AddGetEndpoint(application, CustomCssThemePath, cssThemeContent);
        setupAction += options => options.InjectStylesheet(CustomCssThemePath);

        return application.UseSwaggerUI(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS theme and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="assembly">The assembly where the embedded CSS file is situated.</param>
    /// <param name="cssFilename">The CSS theme filename (e.g. "myCustomTheme.css").</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> or <paramref name="cssFilename"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        Assembly assembly,
        string cssFilename,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(cssFilename);

        var options = new SwaggerUIOptions();
        setupAction?.Invoke(options);

        var theme = FileProvider.GetResourceText(cssFilename, assembly, out var commonTheme, out var loadJs);

        if (!string.IsNullOrEmpty(commonTheme))
        {
            commonTheme = AdvancedOptions.Apply(commonTheme, options.ConfigObject.AdditionalItems, MimeTypes.Text.Css);
            const string CommonCssThemePath = $"{FileProvider.StylesPath}common.css";
            FileProvider.AddGetEndpoint(application, CommonCssThemePath, commonTheme);
            setupAction += options => options.InjectStylesheet(CommonCssThemePath);

            if (loadJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
                setupAction += InjectJavascript(application, options);
        }

        FileProvider.AddGetEndpoint(application, FileProvider.StylesPath + cssFilename, theme);
        setupAction += options => options.InjectStylesheet(FileProvider.StylesPath + cssFilename);

        return application.UseSwaggerUI(setupAction);
    }

    #region Private

    private static Action<SwaggerUIOptions> ConfigureSwaggerUIOptions(IApplicationBuilder application, SwaggerUIOptions options, BaseTheme theme)
    {
        ImportSwaggerTheme(application, options, theme);

        var optionsAction = InjectCommonTheme(application, options, theme);
        optionsAction += InjectTheme(theme);

        if (theme.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
            optionsAction += InjectJavascript(application, options);

        return optionsAction;
    }

    private static void ImportSwaggerTheme(IApplicationBuilder application, SwaggerUIOptions options, BaseTheme theme, bool isCommonTheme = false)
    {
        var themeContent = FileProvider.GetResourceText(theme.FileName, theme.GetType());

        if (isCommonTheme)
            themeContent = AdvancedOptions.Apply(themeContent, options.ConfigObject.AdditionalItems, MimeTypes.Text.Css);

        FileProvider.AddGetEndpoint(application, ComposeThemePath(theme), themeContent);
    }

    private static Action<SwaggerUIOptions> InjectTheme(BaseTheme theme)
    {
        return options => options.InjectStylesheet(ComposeThemePath(theme));
    }

    private static string ComposeThemePath(BaseTheme theme)
    {
        return FileProvider.StylesPath + theme.FileName;
    }

    private static Action<SwaggerUIOptions> InjectCommonTheme(IApplicationBuilder application, SwaggerUIOptions options, BaseTheme theme)
    {
        var commonTheme = theme.Common;
        ImportSwaggerTheme(application, options, commonTheme, true);

        return InjectTheme(commonTheme);
    }

    private static Action<SwaggerUIOptions> InjectJavascript(IApplicationBuilder application, SwaggerUIOptions options)
    {
        var javascript = FileProvider.GetResourceText(FileProvider.JsFilename);
        javascript = AdvancedOptions.Apply(javascript, options.ConfigObject.AdditionalItems, MimeTypes.Text.Javascript);

        const string FullPath = FileProvider.ScriptsPath + FileProvider.JsFilename;
        FileProvider.AddGetEndpoint(application, FullPath, javascript, MimeTypes.Text.Javascript);

        return options => options.InjectJavascript(FullPath);
    }

    #endregion Private
}