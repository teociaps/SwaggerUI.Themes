using AspNetCore.Swagger.Themes;
using NSwag.AspNetCore;
using System.Reflection;
using System.Text;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/> to configure NSwag Swagger UI with themes.
/// </summary>
public static class NSwagBuilderExtensions
{
    /// <summary>
    /// Registers the Swagger UI middleware with the specified theme and optional settings setup action.
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

        return application.UseSwaggerUi(settings =>
        {
            configureSettings?.Invoke(settings);
            ConfigureThemeInternal(application, settings, theme);
        });
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS theme and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="cssThemeContent">The CSS theme to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI settings.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="cssThemeContent"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUi(
        this IApplicationBuilder application,
        string cssThemeContent,
        Action<SwaggerUiSettings> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(cssThemeContent);

        const string CustomPath = $"{FileProvider.StylesPath}custom.css";
        FileProvider.AddGetEndpoint(application, CustomPath, cssThemeContent);
        ThemeSwitcher.RegisterCustomTheme("custom.css", isStandalone: true);

        setupAction += opt => opt.CustomStylesheetPath = CustomPath;
        return application.UseSwaggerUi(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying a CSS theme from an assembly with optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="assembly">The assembly where the embedded CSS file is situated.</param>
    /// <param name="cssFilename">The CSS theme filename (e.g. "myCustomTheme.css").</param>
    /// <param name="configureSettings">An optional action to configure Swagger UI settings.</param>
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

        return application.UseSwaggerUi(settings =>
        {
            configureSettings?.Invoke(settings);

            var (themeContent, commonContent, loadJs, isStandalone, themeName) = ThemeBuilderHelpers.LoadAssemblyTheme(assembly, cssFilename);

            // Create a dynamic BaseTheme instance for this CSS file
            // This allows it to be discovered and included in the theme switcher
            var dynamicTheme = new DynamicTheme(cssFilename, isStandalone, cssFilename.EndsWith(".min.css"));

            if (!isStandalone)
            {
                // Register the theme for auto-discovery
                ThemeSwitcher.RegisterTheme(dynamicTheme, cssFilename, isStandalone);

                commonContent = AdvancedOptions.Apply(commonContent, settings.AdditionalSettings, MimeTypes.Text.Css);

                // NSwag limitation: can only set ONE CustomStylesheetPath
                // So we combine common + theme CSS into a single file
                var combinedContent = commonContent + Environment.NewLine + themeContent;

                var themePath = $"{FileProvider.StylesPath}{cssFilename}";
                FileProvider.AddGetEndpoint(application, themePath, combinedContent);
                settings.CustomStylesheetPath = themePath;

                if (loadJs && AdvancedOptions.AnyJsFeatureEnabled(settings.AdditionalSettings))
                {
                    InjectJavascriptInternal(application, settings);
                    ConfigureThemeSwitcher(application, settings, dynamicTheme);
                }
            }
            else
            {
                // Standalone theme - just the theme CSS
                var themePath = $"{FileProvider.StylesPath}{cssFilename}";
                FileProvider.AddGetEndpoint(application, themePath, themeContent);
                settings.CustomStylesheetPath = themePath;
            }
        });
    }

    private static void ConfigureThemeInternal(
        IApplicationBuilder application,
        SwaggerUiSettings settings,
        BaseTheme theme)
    {
        // Load and register combined theme content (common + theme CSS)
        var themeContent = ThemeSwitcher.LoadThemeContent(theme, settings.AdditionalSettings);
        var themePath = $"{FileProvider.StylesPath}{theme.FileName}";
        FileProvider.AddGetEndpoint(application, themePath, themeContent);

        settings.CustomStylesheetPath = themePath;

        // Configure JS features if enabled
        if (theme.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(settings.AdditionalSettings))
        {
            InjectJavascriptInternal(application, settings);
            ConfigureThemeSwitcher(application, settings, theme);
        }
    }

    private static void InjectJavascriptInternal(
        IApplicationBuilder application,
        SwaggerUiSettings settings)
    {
        var javascript = ThemeBuilderHelpers.GetConfiguredJavaScript(settings.AdditionalSettings);
        ThemeBuilderHelpers.RegisterJavaScriptEndpoint(application, javascript);

        settings.CustomJavaScriptPath = FileProvider.ScriptsPath + FileProvider.JsFilename;
    }

    private static void ConfigureThemeSwitcher(
        IApplicationBuilder application,
        SwaggerUiSettings settings,
        BaseTheme theme)
    {
        var headContent = new StringBuilder();
        var switcherOptions = settings.GetThemeSwitcherOptions();

        ThemeBuilderHelpers.ConfigureThemeWithSwitcher(
            application,
            theme,
            settings.AdditionalSettings,
            switcherOptions,
            availableTheme =>
            {
                var themeCss = ThemeSwitcher.LoadThemeContent(availableTheme, settings.AdditionalSettings);
                var themePath = $"{FileProvider.StylesPath}{availableTheme.FileName}";
                FileProvider.AddGetEndpoint(application, themePath, themeCss);

                headContent.AppendLine(ThemeSwitcher.CreateThemeLink(availableTheme, disabled: true));
            });

        // Mark current theme
        headContent.AppendLine(ThemeSwitcher.CreateCurrentThemeMarkerScript(theme));

        // Append to settings
        settings.CustomHeadContent += headContent.ToString();
    }
}