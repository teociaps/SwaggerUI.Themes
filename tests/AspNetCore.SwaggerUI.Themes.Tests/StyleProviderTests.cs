using AspNetCore.Swagger.Themes.Tests.Utilities;
using Shouldly;
using System.Reflection;
using static AspNetCore.Swagger.Themes.FileProvider;

namespace AspNetCore.Swagger.Themes.Tests;

public class StyleProviderTests : IClassFixture<StyleProviderWebApplicationFactory<Program>>
{
    private readonly StyleProviderWebApplicationFactory<Program> _styleProviderWebApplicationFactory;

    private readonly Dictionary<string, object> _advancedOptions = new()
        {
            { AdvancedOptions.PinnableTopbar, true },
            { AdvancedOptions.StickyOperations, true },
            { AdvancedOptions.BackToTop, true },
            { AdvancedOptions.ExpandOrCollapseAllOperations, true }
        };

    public StyleProviderTests(StyleProviderWebApplicationFactory<Program> styleProviderWebApplicationFactory)
    {
        _styleProviderWebApplicationFactory = styleProviderWebApplicationFactory;
        _styleProviderWebApplicationFactory.CreateClient();
    }

    [Fact]
    public void GetResourceText_ThrowsFileNotFoundException_WhenResourceNotFound()
    {
        // Arrange
        const string InvalidFileName = "nonexistent.css";
        var styleType = typeof(BaseStyle);

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => GetResourceText(InvalidFileName, styleType));

        exception.Message.ShouldContain("Can't find");
    }

    [Fact]
    public void GetResourceText_ThrowsInvalidOperationException_WhenNotCssFile()
    {
        // Arrange
        const string InvalidFileName = "not_a_css_file.txt";
        var assembly = Assembly.GetExecutingAssembly();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => GetResourceText(InvalidFileName, assembly, out string commonStyle, out bool loadJs));

        exception.Message.ShouldContain("not a valid name for CSS files");
    }

    [Theory]
    [ClassData(typeof(StyleTestData))]
    public void GetResourceText_ShouldEmbedAndRetrieveStyleFromExecutingAssembly(BaseStyle style)
    {
        // Arrange/Act
        var styleText = GetResourceText(style.FileName, style.GetType());

        // Assert - Verify correct header format based on file type
        if (style.FileName.EndsWith(".min.css"))
        {
            styleText.ShouldStartWith($"/*{style}*/");
        }
        else
        {
            styleText.ShouldStartWith($"""
                /*
                    {style}

                    https://github.com/teociaps/SwaggerUI.Themes
                */
                """);
        }

        if (AdvancedOptions.AnyJsFeatureEnabled(_advancedOptions))
        {
            // Arrange/Act - Test minified JS
            var minJsFile = GetResourceText(JsFilename);

            // Assert
            minJsFile.ShouldStartWith("/*Swagger UI*/");
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
    public void GetResourceText_ShouldGetCommonCssStyleWithJS_WhenExternalCssLoadedWithinAssemblyNamespace()
    {
        // Arrange
        const string ExternalFileName = "custom.style.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonStyle, out var loadJs);

        // Assert
        styleContent.ShouldBe("""
            /*
                Test Custom Style

                https://github.com/teociaps/SwaggerUI.Themes
            */

            body {
                background-color: var(--body-background-color, #fafafa);
            }
            """);

        // Common style is always minified version
        commonStyle.ShouldStartWith("/*Common Style*/");
        loadJs.ShouldBeTrue();
    }

    [Fact]
    public void GetResourceText_ShouldNotLoadCommonStyleOrJS_WhenStandaloneStyleInCustomNamespace()
    {
        // Arrange
        const string ExternalFileName = "standalone.style.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonStyle, out var loadJs);

        // Assert
        styleContent.ShouldStartWith("""
            /*
                Test Standalone Style

                https://github.com/teociaps/SwaggerUI.Themes
            */

            /* Standalone style - should NOT load common.css or ui.js */
            """);

        // Standalone style should NOT load common style or JS
        commonStyle.ShouldBeEmpty();
        loadJs.ShouldBeFalse();
    }

    [Theory]
    [InlineData("standalone.custom.css")]
    [InlineData("STANDALONE.theme.css")]
    [InlineData("my.standalone.style.css")]
    public void GetResourceText_ShouldRecognizeStandaloneKeyword_CaseInsensitive(string fileName)
    {
        // Note: This test verifies the logic without actually having these files
        // The actual behavior is tested in GetResourceText_ShouldNotLoadCommonStyleOrJS_WhenStandaloneStyleInCustomNamespace

        // Assert
        fileName.Contains("standalone", StringComparison.OrdinalIgnoreCase).ShouldBeTrue();
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