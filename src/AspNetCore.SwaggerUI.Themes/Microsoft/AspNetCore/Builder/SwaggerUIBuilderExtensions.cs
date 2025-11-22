using AspNetCore.Swagger.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/> to configure Swagger UI with themes.
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

        ConfigureTheme(application, options, theme).Invoke(options);
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

        return application.UseSwaggerUI(opt =>
        {
            setupAction?.Invoke(opt);
            ConfigureThemeInternal(application, opt, theme);
        });
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS theme.
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

        const string customPath = $"{FileProvider.StylesPath}custom.css";
        FileProvider.AddGetEndpoint(application, customPath, cssThemeContent);
        ThemeSwitcher.RegisterCustomTheme("custom.css", isStandalone: true);

        setupAction += options => options.InjectStylesheet(customPath);
        return application.UseSwaggerUI(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying a CSS theme from an assembly.
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

        var (themeContent, commonContent, loadJs, isStandalone, themeName) =
            ThemeBuilderHelpers.LoadAssemblyTheme(assembly, cssFilename);

        var customPath = FileProvider.StylesPath + cssFilename;
        ThemeSwitcher.RegisterCustomTheme(cssFilename, isStandalone);

        if (!isStandalone)
        {
            commonContent = AdvancedOptions.Apply(commonContent, options.ConfigObject.AdditionalItems, MimeTypes.Text.Css);

            const string commonPath = $"{FileProvider.StylesPath}common.css";
            FileProvider.AddGetEndpoint(application, commonPath, commonContent);
            setupAction += opt => opt.InjectStylesheet(commonPath);

            if (loadJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
            {
                setupAction += InjectJavascript(application, options);
                setupAction += opt => ThemeBuilderHelpers.ConfigureCustomThemeWithSwitcher(
                    application, themeName, isStandalone, options.ConfigObject.AdditionalItems, opt.GetThemeSwitcherOptions());
            }
        }

        FileProvider.AddGetEndpoint(application, customPath, themeContent);
        setupAction += opt => opt.InjectStylesheet(customPath);

        return application.UseSwaggerUI(setupAction);
    }

    private static void ConfigureThemeInternal(
        IApplicationBuilder application,
        SwaggerUIOptions options,
        BaseTheme theme)
    {
        // Register theme endpoints
        ThemeSwitcher.RegisterThemeEndpoints(application, theme, options.ConfigObject.AdditionalItems);

        // Inject stylesheets
        var commonPath = FileProvider.StylesPath + theme.Common.FileName;
        var themePath = FileProvider.StylesPath + theme.FileName;
        options.InjectStylesheet(commonPath);
        options.InjectStylesheet(themePath);

        // Configure JS features if enabled
        if (theme.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
        {
            InjectJavascriptInternal(application, options);
            ConfigureThemeSwitcher(application, options, theme);
        }
    }

    private static Action<SwaggerUIOptions> ConfigureTheme(
        IApplicationBuilder application,
        SwaggerUIOptions options,
        BaseTheme theme)
    {
        // This is only used by the first overload
        return opt => ConfigureThemeInternal(application, opt, theme);
    }

    private static void InjectJavascriptInternal(
        IApplicationBuilder application,
        SwaggerUIOptions options)
    {
        var javascript = ThemeBuilderHelpers.GetConfiguredJavaScript(options.ConfigObject.AdditionalItems);
        ThemeBuilderHelpers.RegisterJavaScriptEndpoint(application, javascript);

        const string jsPath = FileProvider.ScriptsPath + FileProvider.JsFilename;
        options.InjectJavascript(jsPath);
    }

    private static Action<SwaggerUIOptions> InjectJavascript(
        IApplicationBuilder application,
        SwaggerUIOptions options)
    {
        return opt => InjectJavascriptInternal(application, opt);
    }

    private static void ConfigureThemeSwitcher(
        IApplicationBuilder application,
        SwaggerUIOptions options,
        BaseTheme theme)
    {
        var headContent = new StringBuilder();

        // Get switcher options from cache
        var switcherOptions = options.GetThemeSwitcherOptions();

        ThemeBuilderHelpers.ConfigureThemeWithSwitcher(
            application,
            theme,
            options.ConfigObject.AdditionalItems,
            switcherOptions,
            availableTheme =>
            {
                // Register endpoint for each available theme
                ThemeSwitcher.RegisterThemeEndpoints(application, availableTheme, options.ConfigObject.AdditionalItems);

                // Inject as disabled stylesheet
                headContent.AppendLine(ThemeSwitcher.CreateThemeLink(availableTheme, disabled: true));
            });

        // Mark current theme
        headContent.AppendLine(ThemeSwitcher.CreateCurrentThemeMarkerScript(theme));

        // Append to options
        options.HeadContent += headContent.ToString();
    }
}