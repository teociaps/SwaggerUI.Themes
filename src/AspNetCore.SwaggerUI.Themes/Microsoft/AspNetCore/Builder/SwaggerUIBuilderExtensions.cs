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
    /// Registers the Swagger UI middleware with a specified style and optional configuration.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="options">The Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        BaseStyle style,
        SwaggerUIOptions options)
    {
        ArgumentNullException.ThrowIfNull(style);

        options ??= new SwaggerUIOptions();
        ConfigureSwaggerUIOptions(application, options, style).Invoke(options);

        return application.UseSwaggerUI(options);
    }

    /// <summary>
    /// Registers the Swagger UI middleware with a specified style and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="style">The style to apply.</param>
    /// <param name="setupAction">An optional action to configure Swagger UI options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        BaseStyle style,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(style);

        var options = new SwaggerUIOptions();
        setupAction?.Invoke(options);

        var optionsAction = ConfigureSwaggerUIOptions(application, options, style);

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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="cssStyleContent"/> is null.</exception>
    public static IApplicationBuilder UseSwaggerUI(
        this IApplicationBuilder application,
        string cssStyleContent,
        Action<SwaggerUIOptions> setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(cssStyleContent);

        const string CustomCssStylePath = $"{FileProvider.StylesPath}custom.css";
        FileProvider.AddGetEndpoint(application, CustomCssStylePath, cssStyleContent);
        setupAction += options => options.InjectStylesheet(CustomCssStylePath);

        return application.UseSwaggerUI(setupAction);
    }

    /// <summary>
    /// Registers the Swagger UI middleware applying the provided CSS style and optional setup action.
    /// </summary>
    /// <param name="application">The application builder instance.</param>
    /// <param name="assembly">The assembly where the embedded CSS file is situated.</param>
    /// <param name="cssFilename">The CSS style filename (e.g. "myCustomStyle.css").</param>
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

        var stylesheet = FileProvider.GetResourceText(cssFilename, assembly, out var commonStyle, out var loadModernJs);

        if (!string.IsNullOrEmpty(commonStyle))
        {
            commonStyle = AdvancedOptions.Apply(commonStyle, options.ConfigObject.AdditionalItems, MimeTypes.Text.Css);
            const string CommonCssStylePath = $"{FileProvider.StylesPath}common.css";
            FileProvider.AddGetEndpoint(application, CommonCssStylePath, commonStyle);
            setupAction += options => options.InjectStylesheet(CommonCssStylePath);

            if (loadModernJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
                setupAction += InjectModernJavascript(application, options);
        }

        FileProvider.AddGetEndpoint(application, FileProvider.StylesPath + cssFilename, stylesheet);
        setupAction += options => options.InjectStylesheet(FileProvider.StylesPath + cssFilename);

        return application.UseSwaggerUI(setupAction);
    }

    #region Private

    private static Action<SwaggerUIOptions> ConfigureSwaggerUIOptions(IApplicationBuilder application, SwaggerUIOptions options, BaseStyle style)
    {
        ImportSwaggerStyle(application, options, style);

        var optionsAction = InjectCommonStyle(application, options, style);
        optionsAction += InjectStyle(style);

        if (style.LoadAdditionalJs && AdvancedOptions.AnyJsFeatureEnabled(options.ConfigObject.AdditionalItems))
            optionsAction += InjectModernJavascript(application, options);

        return optionsAction;
    }

    private static void ImportSwaggerStyle(IApplicationBuilder application, SwaggerUIOptions options, BaseStyle style, bool isCommonStyle = false)
    {
        var stylesheet = FileProvider.GetResourceText(style.FileName, style.GetType());

        if (isCommonStyle)
            stylesheet = AdvancedOptions.Apply(stylesheet, options.ConfigObject.AdditionalItems, MimeTypes.Text.Css);

        FileProvider.AddGetEndpoint(application, ComposeStylePath(style), stylesheet);
    }

    private static Action<SwaggerUIOptions> InjectStyle(BaseStyle style)
    {
        return options => options.InjectStylesheet(ComposeStylePath(style));
    }

    private static string ComposeStylePath(BaseStyle style)
    {
        return FileProvider.StylesPath + style.FileName;
    }

    private static Action<SwaggerUIOptions> InjectCommonStyle(IApplicationBuilder application, SwaggerUIOptions options, BaseStyle style)
    {
        var commonStyle = style.Common;
        ImportSwaggerStyle(application, options, commonStyle, true);

        return InjectStyle(commonStyle);
    }

    private static Action<SwaggerUIOptions> InjectModernJavascript(IApplicationBuilder application, SwaggerUIOptions options)
    {
        const string JsFilename = "modern.js";
        var javascript = FileProvider.GetResourceText(JsFilename);
        javascript = AdvancedOptions.Apply(javascript, options.ConfigObject.AdditionalItems, MimeTypes.Text.Javascript);

        const string FullPath = FileProvider.ScriptsPath + JsFilename;
        FileProvider.AddGetEndpoint(application, FullPath, javascript, MimeTypes.Text.Javascript);

        return options => options.InjectJavascript(FullPath);
    }

    #endregion Private
}