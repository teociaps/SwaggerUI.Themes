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

            settings.CustomInlineStyles = ThemeSwitcher.LoadThemeContent(theme, settings.AdditionalSettings);

            if (theme.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(settings.AdditionalSettings))
            {
                InjectJavascript(application, settings);
                ConfigureThemeSwitcher(application, settings, theme);
            }
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

        ThemeSwitcher.RegisterCustomTheme("custom.css", isStandalone: true);

        setupAction += opt => opt.CustomInlineStyles = cssThemeContent;
        return application.UseSwaggerUi(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying a CSS theme from an assembly with optional setup action.
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

        return application.UseSwaggerUi(settings =>
        {
            configureSettings?.Invoke(settings);

            var (themeContent, commonContent, loadJs, isStandalone, themeName) =
                ThemeBuilderHelpers.LoadAssemblyTheme(assembly, cssFilename);

            // Create a dynamic BaseTheme instance for this CSS file
            // This allows it to be discovered and included in the theme switcher
            var dynamicTheme = new DynamicTheme(cssFilename, isStandalone, cssFilename.EndsWith(".min.css"));

            if (!isStandalone)
            {
                // Register the theme for auto-discovery
                ThemeSwitcher.RegisterTheme(dynamicTheme, cssFilename, isStandalone);

                commonContent = AdvancedOptions.Apply(commonContent, settings.AdditionalSettings, MimeTypes.Text.Css);
                themeContent = commonContent + Environment.NewLine + themeContent;

                if (loadJs && AdvancedOptions.AnyJsFeatureEnabled(settings.AdditionalSettings))
                {
                    InjectJavascript(application, settings);
                    ConfigureThemeSwitcher(application, settings, dynamicTheme);
                }
            }

            settings.CustomInlineStyles = themeContent;
        });
    }

    private static void InjectJavascript(IApplicationBuilder application, SwaggerUiSettings settings)
    {
        var javascript = ThemeBuilderHelpers.GetConfiguredJavaScript(settings.AdditionalSettings);
        ThemeBuilderHelpers.RegisterJavaScriptEndpoint(application, javascript);

        const string jsPath = FileProvider.ScriptsPath + FileProvider.JsFilename;
        settings.CustomJavaScriptPath = jsPath;
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

        settings.CustomHeadContent += headContent.ToString();
    }
}