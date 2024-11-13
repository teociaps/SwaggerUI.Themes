using Shouldly;
using System.Reflection;
using static AspNetCore.Swagger.Themes.FileProvider;

namespace AspNetCore.Swagger.Themes.Tests;

public class StyleProviderTests : IClassFixture<StyleProviderWebApplicationFactory<Program>>
{
    private readonly StyleProviderWebApplicationFactory<Program> _styleProviderWebApplicationFactory;

    public StyleProviderTests(StyleProviderWebApplicationFactory<Program> styleProviderWebApplicationFactory)
    {
        _styleProviderWebApplicationFactory = styleProviderWebApplicationFactory;
        _styleProviderWebApplicationFactory.CreateClient();
    }

    [Theory]
    [ClassData(typeof(StyleTestData))]
    public void GetResourceText_ShouldEmbedAndRetrieveStyleFromExecutingAssembly(BaseStyle style)
    {
        // Arrange/Act
        var styleText = GetResourceText(style.FileName, style.GetType());

        // Assert
        styleText.ShouldStartWith($"""
            /*
                {style}

                https://github.com/teociaps/SwaggerUI.Themes
            */
            """);

        if (style is ModernStyle modernStyle && modernStyle.LoadAdditionalJs)
        {
            // Arrange/Act
            var jsFile = GetResourceText("modern.js");

            // Assert
            jsFile.ShouldStartWith("""
            /*
                Modern UI

                https://github.com/teociaps/SwaggerUI.Themes
            */
            """);
        }
    }

    [Fact]
    public void GetResourceText_ShouldThrowFileNotFoundException_WhenExternalCssWithoutDeclaredStyle()
    {
        // Arrange
        const string ExternalFileName = "style.css";

        // Act/Assert
        Should.Throw(() => GetResourceText(ExternalFileName), typeof(FileNotFoundException));
    }

    [Fact]
    public void GetResourceText_ShouldGetCssStyle_WhenExternalCssLoadedWithinAssemblyNamespace()
    {
        // Arrange
        const string ExternalFileName = "style.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonClassicStyle, out var isModernStyle);

        // Assert
        styleContent.ShouldBe("""
            /*
                Test Style

                https://github.com/teociaps/SwaggerUI.Themes
            */

            body {
                background-color: var(--body-background-color, #fafafa);
            }
            """);

        commonClassicStyle.ShouldBeEmpty();
        isModernStyle.ShouldBeFalse();
    }


    [Fact]
    public void GetResourceText_ShouldGetCommonClassicCssStyle_WhenExternalCssHasClassicPrefix()
    {
        // Arrange
        const string ExternalFileName = "classic.style.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonClassicStyle, out var isModernStyle);

        // Assert
        styleContent.ShouldBe("""
            /*
                Test Classic Style

                https://github.com/teociaps/SwaggerUI.Themes
            */

            body {
                background-color: var(--body-background-color, #fafafa);
            }
            """);

        commonClassicStyle.ShouldStartWith("""
            /*
                Common Style

                https://github.com/teociaps/SwaggerUI.Themes
            */
            """);

        isModernStyle.ShouldBeFalse();
    }


    [Fact]
    public void GetResourceText_ShouldGetCommonModernCssStyle_WhenExternalCssHasModernPrefix()
    {
        // Arrange
        const string ExternalFileName = "modern.style.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonModernStyle, out var isModernStyle);

        // Assert
        styleContent.ShouldBe("""
            /*
                Test Modern Style

                https://github.com/teociaps/SwaggerUI.Themes
            */

            body {
                background-color: var(--body-background-color, #fafafa);
            }
            """);

        commonModernStyle.ShouldStartWith("""
            /*
                Modern Common Style

                https://github.com/teociaps/SwaggerUI.Themes
            */
            """);

        isModernStyle.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(StyleTestData))]
    public async Task AddGetEndpoint_ShouldReturnStyleContent_WhenWebApplication(BaseStyle style)
    {
        // Arrange
        var fullPath = StylesPath + style.FileName;
        var styleText = GetResourceText(style.FileName, style.GetType());

        // Act
        var response = await _styleProviderWebApplicationFactory.Client.GetAsync(fullPath);

        // Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBeEquivalentTo(styleText);
    }

    [Theory]
    [ClassData(typeof(StyleTestData))]
    public async Task AddGetEndpoint_ShouldReturnCssContent_WhenNotWebApplication(BaseStyle style)
    {
        // Arrange
        var mockAppBuilder = new MockApplicationBuilder();
        var path = StylesPath + style.FileName;
        var content = GetResourceText(style.FileName, style.GetType());

        // Act
        AddGetEndpoint(mockAppBuilder, path, content);
        var app = mockAppBuilder.Build();

        // Simulate a request
        var context = MockApplicationBuilder.CreateHttpContext(path);
        await app.Invoke(context);

        await context.Response.Body.FlushAsync();

        // Assert
        Assert.Equal(200, context.Response.StatusCode);
        Assert.Equal(MimeTypes.Text.Css, context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        Assert.Equal(content, responseBody);
    }
}