using AspNetCore.Swagger.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class StyleSwaggerUIBuilderExtensions
{
    /// <summary>
    /// Registers the Swagger UI middleware with a specified style and optional configuration.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="options">The Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        BaseStyle style,
        SwaggerUIOptions options)
    {
        ArgumentNullException.ThrowIfNull(style);

        options ??= new SwaggerUIOptions();
        ConfigureSwaggerUIOptions(application, style).Invoke(options);

        return application.UseSwaggerUI(options);
    }

    /// <summary>
    /// Registers the Swagger UI middleware with a specified style and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        BaseStyle style,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(style);

        var optionsAction = ConfigureSwaggerUIOptions(application, style);

        if (setupAction is not null)
            optionsAction += setupAction;

        return application.UseSwaggerUI(optionsAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS style and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="cssStyleContent">The CSS style to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        string cssStyleContent,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(cssStyleContent);

        const string CustomCssStylePath = "/styles/custom.css";
        FileProvider.AddGetEndpoint(application, CustomCssStylePath, cssStyleContent);
        setupAction += options => options.InjectStylesheet(CustomCssStylePath);

        return application.UseSwaggerUI(setupAction);
    }

    #region Private

    private static Action<SwaggerUIOptions> ConfigureSwaggerUIOptions(IApplicationBuilder app, BaseStyle style)
    {
        ImportSwaggerStyle(app, style);

        var optionsAction = InjectCommonStyle(app, style);
        optionsAction += InjectStyle(style);

        if (style.IsModern)
            optionsAction += InjectModernJavaScript(app);

        return optionsAction;
    }

    private static void ImportSwaggerStyle(IApplicationBuilder app, BaseStyle style)
    {
        var stylesheet = FileProvider.GetResourceText(style.FileName, style.GetType());
        FileProvider.AddGetEndpoint(app, ComposeStylePath(style), stylesheet);
    }

    private static Action<SwaggerUIOptions> InjectStyle(BaseStyle style)
    {
        return options => options.InjectStylesheet(ComposeStylePath(style));
    }

    private static string ComposeStylePath(BaseStyle style)
    {
        return FileProvider.StylesPath + style.FileName;
    }

    private static Action<SwaggerUIOptions> InjectCommonStyle(IApplicationBuilder app, BaseStyle style)
    {
        var commonStyle = style.Common;
        ImportSwaggerStyle(app, commonStyle);

        return InjectStyle(commonStyle);
    }

    private static Action<SwaggerUIOptions> InjectModernJavaScript(IApplicationBuilder app)
    {
        const string JsFilename = "modern.js";
        var javascript = FileProvider.GetResourceText(JsFilename);
        const string FullPath = FileProvider.ScriptsPath + JsFilename;

        FileProvider.AddGetEndpoint(app, FullPath, javascript, MimeTypes.Text.Javascript);
        return options => options.InjectJavascript(FullPath);
    }

    #endregion Private
}