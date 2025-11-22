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

        ConfigureTheme(application, theme).Invoke(options);
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

        const string CustomPath = $"{FileProvider.StylesPath}custom.css";
        FileProvider.AddGetEndpoint(application, CustomPath, cssThemeContent);
        ThemeSwitcher.RegisterCustomTheme("custom.css", isStandalone: true);

        setupAction += options => options.InjectStylesheet(CustomPath);
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

        return application.UseSwaggerUI(opt =>
        {
            setupAction?.Invoke(opt);

            var (themeContent, commonContent, loadJs, isStandalone, themeName) = ThemeBuilderHelpers.LoadAssemblyTheme(assembly, cssFilename);

            // Create a dynamic BaseTheme instance for this CSS file
            // This allows it to be discovered and included in the theme switcher
            var dynamicTheme = new DynamicTheme(cssFilename, isStandalone, cssFilename.EndsWith(".min.css"));

            if (!isStandalone)
            {
                // Register the theme for auto-discovery
                ThemeSwitcher.RegisterTheme(dynamicTheme, cssFilename, isStandalone);

                commonContent = AdvancedOptions.Apply(commonContent, opt.ConfigObject.AdditionalItems, MimeTypes.Text.Css);

                const string CommonPath = $"{FileProvider.StylesPath}common.css";
                FileProvider.AddGetEndpoint(application, CommonPath, commonContent);
                opt.InjectStylesheet(CommonPath);

                if (loadJs && AdvancedOptions.AnyJsFeatureEnabled(opt.ConfigObject.AdditionalItems))
                {
                    InjectJavascriptInternal(application, opt);
                    ConfigureThemeSwitcher(application, opt, dynamicTheme);
                }
            }

            var customPath = FileProvider.StylesPath + cssFilename;
            FileProvider.AddGetEndpoint(application, customPath, themeContent);
            opt.InjectStylesheet(customPath);
        });
    }

    private static void ConfigureThemeInternal(
        IApplicationBuilder application,
        SwaggerUIOptions options,
        BaseTheme theme)
    {
        // Register theme endpoints
        ThemeSwitcher.RegisterThemeEndpoints(application, theme, options.ConfigObject.AdditionalItems);

        // Inject stylesheets
        options.InjectStylesheet(FileProvider.StylesPath + theme.Common.FileName);
        options.InjectStylesheet(FileProvider.StylesPath + theme.FileName);

        // Configure JS features if enabled
        if (theme.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
        {
            InjectJavascriptInternal(application, options);
            ConfigureThemeSwitcher(application, options, theme);
        }
    }

    private static Action<SwaggerUIOptions> ConfigureTheme(
        IApplicationBuilder application,
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

        options.InjectJavascript(FileProvider.ScriptsPath + FileProvider.JsFilename);
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