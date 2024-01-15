using AspNetCore.SwaggerUI.Themes;
using Microsoft.AspNetCore.Builder;

namespace Swashbuckle.AspNetCore.SwaggerUI;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class StyleSwaggerUIBuilderExtensions
{
    private const string DarkStyleFile = "dark.css";

    /// <summary>
    /// Register the SwaggerUI middleware using dark mode. You can override the behavior by
    /// providing options.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="options">The SwaggerUI options.</param>
    public static IApplicationBuilder UseDarkSwaggerUI(this WebApplication app, SwaggerUIOptions options)
    {
        ImportSwaggerStyle(app);

        SetDarkMode(options);
        return app.UseSwaggerUI(options);
    }

    /// <summary>
    /// Register the SwaggerUI middleware using dark mode with optional setup action for DI-injected options.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="setupAction">An action used to configure SwaggerUI options.</param>
    public static IApplicationBuilder UseDarkSwaggerUI(
        this WebApplication app,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ImportSwaggerStyle(app);

        var defaultSetupAction = SetDarkMode();
        if (setupAction is not null)
        {
            defaultSetupAction = options =>
            {
                defaultSetupAction(options);
                setupAction(options);
            };
        }

        return app.UseSwaggerUI(defaultSetupAction);
    }

    private static void ImportSwaggerStyle(WebApplication app)
    {
        string darkCss = StyleProvider.GetResourceText(DarkStyleFile);

        StyleProvider.AddGetEndpoint(app, "/styles/dark.css", darkCss);
    }

    private static void SetDarkMode(SwaggerUIOptions swaggerUiOptions)
    {
        swaggerUiOptions.InjectDarkThemeStylesheet();
    }

    private static Action<SwaggerUIOptions> SetDarkMode()
    {
        return x => x.InjectDarkThemeStylesheet();
    }

    private static void InjectDarkThemeStylesheet(this SwaggerUIOptions swaggerUiOptions)
    {
        swaggerUiOptions.InjectStylesheet("/styles/dark.css");
    }
}