using AspNetCore.SwaggerUI.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class StyleSwaggerUIBuilderExtensions
{
    /// <summary>
    /// Register the SwaggerUI middleware using the specified style. You can override the behavior
    /// by providing options.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="options">The SwaggerUI options.</param>
    public static IApplicationBuilder UseSwaggerUI(this WebApplication app, Style style, SwaggerUIOptions options)
    {
        ImportSwaggerStyle(app, style);

        InjectStyle(style).Invoke(options);
        return app.UseSwaggerUI(options);
    }

    /// <summary>
    /// Register the SwaggerUI middleware using the specified style with optional setup action for
    /// DI-injected options.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="setupAction">An action used to configure SwaggerUI options.</param>
    public static IApplicationBuilder UseSwaggerUI(
        this WebApplication app,
        Style style,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ImportSwaggerStyle(app, style);

        var defaultSetupAction = InjectStyle(style);
        if (setupAction is not null)
            defaultSetupAction += setupAction;

        return app.UseSwaggerUI(defaultSetupAction);
    }

    private static void ImportSwaggerStyle(WebApplication app, Style style)
    {
        string stylesheet = StyleProvider.GetResourceText(style.FileName);

        StyleProvider.AddGetEndpoint(app, ComposeFullStylePath(style), stylesheet, style.FormatText);
    }

    private static Action<SwaggerUIOptions> InjectStyle(Style style)
    {
        return x => x.InjectStylesheet(ComposeFullStylePath(style));
    }

    private static string ComposeFullStylePath(Style style)
    {
        return StyleProvider.StylePath + style.FileName;
    }
}