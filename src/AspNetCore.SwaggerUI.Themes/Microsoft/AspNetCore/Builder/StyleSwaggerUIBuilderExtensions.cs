using AspNetCore.Swagger.Themes;
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
    public static IApplicationBuilder UseSwaggerUI(
        this WebApplication app,
        BaseStyle style,
        SwaggerUIOptions options)
    {
        // Common style
        InjectCommonStyle(app, style).Invoke(options);

        // Chosen style
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
        BaseStyle style,
        Action<SwaggerUIOptions> setupAction = null)
    {
        // Common style
        var defaultSetupAction = InjectCommonStyle(app, style);

        // Chosen style
        ImportSwaggerStyle(app, style);

        defaultSetupAction += InjectStyle(style);
        if (setupAction is not null)
            defaultSetupAction += setupAction;

        return app.UseSwaggerUI(defaultSetupAction);
    }

    private static void ImportSwaggerStyle(WebApplication app, BaseStyle style)
    {
        string stylesheet = FileProvider.GetResourceText(style.FileName);

        FileProvider.AddGetEndpoint(app, ComposeFullStylePath(style), stylesheet);
    }

    private static Action<SwaggerUIOptions> InjectStyle(BaseStyle style)
    {
        return x => x.InjectStylesheet(ComposeFullStylePath(style));
    }

    private static string ComposeFullStylePath(BaseStyle style)
    {
        return FileProvider.StylesPath + style.FileName;
    }

    private static Action<SwaggerUIOptions> InjectCommonStyle(WebApplication app, BaseStyle style)
    {
        var commonStyle = style.Common;
        ImportSwaggerStyle(app, commonStyle);

        var action = InjectStyle(commonStyle);
        if (style.IsModern)
            action += InjectModernJavaScript(app);

        return action;
    }

    private static Action<SwaggerUIOptions> InjectModernJavaScript(WebApplication app)
    {
        const string JsFilename = "modern.js";
        string javascript = FileProvider.GetResourceText(JsFilename);
        const string FullPath = FileProvider.ScriptsPath + JsFilename;

        FileProvider.AddGetEndpoint(app, FullPath, javascript, MimeTypes.Text.Javascript);
        return x => x.InjectJavascript(FullPath);
    }
}