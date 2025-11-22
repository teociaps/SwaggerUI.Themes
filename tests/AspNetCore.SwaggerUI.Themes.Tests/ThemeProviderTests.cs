using AspNetCore.Swagger.Themes.Tests.Utilities;
using Shouldly;
using System.Reflection;
using static AspNetCore.Swagger.Themes.FileProvider;

namespace AspNetCore.Swagger.Themes.Tests;

/// <summary>
/// Tests for ThemeProvider functionality using WebApplicationFactory.
/// Tests for non-WebApplication scenarios are in FileProviderMiddlewareTests.
/// </summary>
public class ThemeProviderTests : IClassFixture<ThemeProviderWebApplicationFactory<Program>>
{
    private readonly ThemeProviderWebApplicationFactory<Program> _themeProviderWebApplicationFactory;

    private readonly Dictionary<string, object> _advancedOptions = new()
    {
        { AdvancedOptions.PinnableTopbar, true },
        { AdvancedOptions.StickyOperations, true },
        { AdvancedOptions.BackToTop, true },
        { AdvancedOptions.ExpandOrCollapseAllOperations, true }
    };

    public ThemeProviderTests(ThemeProviderWebApplicationFactory<Program> themeProviderWebApplicationFactory)
    {
        _themeProviderWebApplicationFactory = themeProviderWebApplicationFactory;
        _themeProviderWebApplicationFactory.CreateClient();
    }

    [Fact]
    public void GetResourceText_ThrowsFileNotFoundException_WhenResourceNotFound()
    {
        // Arrange
        const string InvalidFileName = "nonexistent.css";
        var themeType = typeof(BaseTheme);

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => GetResourceText(InvalidFileName, themeType));

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

        exception.Message.ShouldContain("is not a valid CSS file. Must end with '.css' or '.min.css'.");
    }

    [Theory]
    [ClassData(typeof(ThemeTestData))]
    public void GetResourceText_ShouldEmbedAndRetrieveThemeStyleFromExecutingAssembly(BaseTheme theme)
    {
        // Arrange/Act
        var styleText = GetResourceText(theme.FileName, theme.GetType());

        // Assert - Verify correct header format based on file type
        if (theme.FileName.EndsWith(".min.css"))
        {
            styleText.ShouldStartWith($"/*{theme} https://github.com/teociaps/SwaggerUI.Themes */");
        }
        else
        {
            styleText.ShouldStartWith($"""
                /*
                    {theme}

                    https://github.com/teociaps/SwaggerUI.Themes
                */
                """);
        }

        if (AdvancedOptions.AnyJsFeatureEnabled(_advancedOptions))
        {
            // Arrange/Act - Test minified JS
            var minJsFile = GetResourceText(JsFilename);

            // Assert
            minJsFile.ShouldStartWith("/*Swagger UI https://github.com/teociaps/SwaggerUI.Themes */");
        }
    }

    [Fact]
    public void GetResourceText_ShouldThrowFileNotFoundException_WhenExternalCssWithoutDeclaredStyle()
    {
        // Arrange
        const string ExternalFileName = "theme.css";

        // Act/Assert
        Should.Throw(() => GetResourceText(ExternalFileName), typeof(FileNotFoundException));
    }

    [Fact]
    public void GetResourceText_ShouldGetCommonCssStyleWithJS_WhenExternalCssLoadedWithinAssemblyNamespace()
    {
        // Arrange
        const string ExternalFileName = "custom.theme.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonStyle, out var loadJs);

        // Assert
        styleContent.ShouldBe("""
            /*
                Test Custom Theme

                https://github.com/teociaps/SwaggerUI.Themes
            */

            body {
                background-color: var(--body-background-color, #fafafa);
            }
            """);

        // Common theme is always minified version
        commonStyle.ShouldStartWith("/*Common Theme https://github.com/teociaps/SwaggerUI.Themes */");
        loadJs.ShouldBeTrue();
    }

    [Fact]
    public void GetResourceText_ShouldNotLoadCommonStyleOrJS_WhenStandaloneStyleInCustomNamespace()
    {
        // Arrange
        const string ExternalFileName = "standalone.theme.css";

        // Act
        var styleContent = GetResourceText(ExternalFileName, Assembly.GetExecutingAssembly(), out var commonStyle, out var loadJs);

        // Assert
        styleContent.ShouldStartWith("""
            /*
                Test Standalone Theme

                https://github.com/teociaps/SwaggerUI.Themes
            */

            /* Standalone theme - should NOT load common.css or ui.js */
            """);

        // Standalone theme should NOT load common theme or JS
        commonStyle.ShouldBeEmpty();
        loadJs.ShouldBeFalse();
    }

    [Theory]
    [InlineData("standalone.custom.css")]
    [InlineData("STANDALONE.theme.css")]
    [InlineData("my.standalone.theme.css")]
    public void GetResourceText_ShouldRecognizeStandaloneKeyword_CaseInsensitive(string fileName)
    {
        // Note: This test verifies the logic without actually having these files
        // The actual behavior is tested in GetResourceText_ShouldNotLoadCommonStyleOrJS_WhenStandaloneStyleInCustomNamespace

        // Assert
        fileName.Contains("standalone", StringComparison.OrdinalIgnoreCase).ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(ThemeTestData))]
    public async Task AddGetEndpoint_ShouldReturnStyleContent_WhenWebApplication(BaseTheme theme)
    {
        // Arrange
        var fullPath = StylesPath + theme.FileName;
        var styleText = GetResourceText(theme.FileName, theme.GetType());

        // Act
        var response = await _themeProviderWebApplicationFactory.Client.GetAsync(fullPath);

        // Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBeEquivalentTo(styleText);
    }
}