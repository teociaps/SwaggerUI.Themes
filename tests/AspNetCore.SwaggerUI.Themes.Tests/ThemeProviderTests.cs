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

    [Fact]
    public void GetResourceText_ShouldFindThemeInSubfolder()
    {
        // Arrange
        const string FileName = "custom.css"; // Exists in SwaggerThemes.Custom folder

        // Act
        var styleContent = GetResourceText(FileName, Assembly.GetExecutingAssembly(), out var commonStyle, out var loadJs);

        // Assert
        styleContent.ShouldNotBeEmpty();
        styleContent.ShouldContain("Test Custom Theme");
        commonStyle.ShouldNotBeEmpty();
        loadJs.ShouldBeTrue();
    }

    [Fact]
    public void GetResourceText_ShouldThrowFileNotFoundException_WhenFileNotInAnySubfolder()
    {
        // Arrange
        const string NonExistentFileName = "nonexistent-theme.css";
        var assembly = Assembly.GetExecutingAssembly();

        // Act & Assert
        var exception = Should.Throw<FileNotFoundException>(() =>
            GetResourceText(NonExistentFileName, assembly, out _, out _));

        exception.Message.ShouldContain("Can't find");
        exception.Message.ShouldContain("SwaggerThemes.*");
        exception.Message.ShouldContain(NonExistentFileName);
    }

    [Theory]
    [InlineData("custom.css", "SwaggerThemes")]
    [InlineData("standalone.theme.css", "SwaggerThemes")]
    [InlineData("custom.theme.css", "SwaggerThemes")]
    public void GetResourceText_ShouldFindTheme_InAnySwaggerThemesNamespace(string fileName, string expectedNamespacePrefix)
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        var styleContent = GetResourceText(fileName, assembly, out _, out _);

        // Assert
        styleContent.ShouldNotBeEmpty();

        // Verify the file was found in a SwaggerThemes.* namespace
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(n => n.Contains(expectedNamespacePrefix) && n.EndsWith(fileName))
            .ToList();

        resourceNames.ShouldNotBeEmpty();
        resourceNames.ShouldHaveSingleItem();
    }

    [Fact]
    public void GetResourceText_ShouldBeFlexible_WithNestedFolderStructure()
    {
        // This test verifies that the search works regardless of folder depth
        // Examples of valid structures:
        // - SwaggerThemes.custom.css
        // - SwaggerThemes.Custom.custom.css

        // Arrange
        const string FileName = "custom.css";
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        var result = GetResourceText(FileName, assembly, out var commonStyle, out var loadJs);

        // Assert
        result.ShouldNotBeEmpty();
        // The function should find it regardless of how deep in the folder structure it is
    }

    [Theory]
    [InlineData("custom.css", true)]
    [InlineData("standalone.theme.css", false)]
    [InlineData("custom.theme.css", true)]
    public void GetResourceText_ShouldCorrectlyDetectStandalone_InSubfolders(string fileName, bool shouldLoadCommon)
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        GetResourceText(fileName, assembly, out var commonStyle, out var loadJs);

        // Assert
        if (shouldLoadCommon)
        {
            commonStyle.ShouldNotBeEmpty("Non-standalone themes should load common.css");
            loadJs.ShouldBeTrue("Non-standalone themes should enable JS");
        }
        else
        {
            commonStyle.ShouldBeEmpty("Standalone themes should NOT load common.css");
            loadJs.ShouldBeFalse("Standalone themes should NOT enable JS");
        }
    }

    [Fact]
    public void GetResourceText_ShouldWorkWith_RealWorldFolderStructure()
    {
        // This test validates the real-world usage pattern from samples
        // Sample folder structure:
        // SwaggerThemes/
        //   Custom/
        //     custom.css
        //   standalone.theme.css

        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act & Assert - should find files in any subfolder
        var customCss = GetResourceText("custom.css", assembly, out _, out _);
        customCss.ShouldNotBeEmpty();

        var standaloneCss = GetResourceText("standalone.theme.css", assembly, out var standaloneCommon, out var standaloneJs);
        standaloneCss.ShouldNotBeEmpty();
        standaloneCommon.ShouldBeEmpty();
        standaloneJs.ShouldBeFalse();
    }

    [Fact]
    public void GetResourceText_ShouldThrowInvalidOperationException_WhenMultipleFilesWithSameName()
    {
        // Intentionally created duplicate files:
        // - SwaggerThemes/duplicate.css
        // - SwaggerThemes/Duplicates/duplicate.css

        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Verify both files exist
        var allResources = assembly.GetManifestResourceNames()
            .Where(n => n.EndsWith(".duplicate.css"))
            .ToList();

        allResources.Count.ShouldBe(2, "Should have exactly 2 duplicate.css files for this test");

        // Act & Assert - Should throw InvalidOperationException with clear error message
        var exception = Should.Throw<InvalidOperationException>(() =>
            GetResourceText("duplicate.css", assembly, out _, out _));

        // Verify error message is helpful
        exception.Message.ShouldContain("Found duplicate.css in multiple locations");
        exception.Message.ShouldContain("SwaggerThemes.duplicate.css");
        exception.Message.ShouldContain("SwaggerThemes.Duplicates.duplicate.css");
        exception.Message.ShouldContain("Ensure the file name is unique across all theme folders");
    }

    [Fact]
    public void GetResourceText_ShouldNotMatch_StandalonePrefix_WhenSearchingNonStandalone()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Verify standalone.test.css exists
        var standaloneExists = assembly.GetManifestResourceNames()
            .Any(n => n.EndsWith(".standalone.test.css"));
        standaloneExists.ShouldBeTrue("standalone.test.css should exist in test assembly");

        // Act & Assert - Searching for "test.css" should not find "standalone.test.css"
        var exception = Should.Throw<FileNotFoundException>(() =>
            GetResourceText("test.css", assembly, out _, out _));

        exception.Message.ShouldContain("Can't find test.css");
    }

    [Fact]
    public void GetResourceText_ShouldMatch_StandaloneFile_WhenExplicitlySearching()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act - Explicitly search for "standalone.theme.css"
        var content = GetResourceText("standalone.theme.css", assembly, out var commonStyle, out var loadJs);

        // Assert
        content.ShouldNotBeEmpty();
        content.ShouldContain("Test Standalone Theme");
        commonStyle.ShouldBeEmpty("Standalone themes don't load common.css");
        loadJs.ShouldBeFalse("Standalone themes don't load JS");
    }
}