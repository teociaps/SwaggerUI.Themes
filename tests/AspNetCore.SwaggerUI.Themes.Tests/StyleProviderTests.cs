using Shouldly;
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
    public void Should_Embed_And_Retrieve_Style_From_ExecutingAssembly(BaseStyle style)
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

        if (style.IsModern)
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
    public void Should_Throw_FileNotFoundException_When_External_Css_Without_Declared_Style()
    {
        // Arrange
        const string ExternalFileName = "style.css";

        // Act/Assert
        Should.Throw(() => GetResourceText(ExternalFileName), typeof(FileNotFoundException));
    }

    [Theory]
    [ClassData(typeof(StyleTestData))]
    public async Task Should_Add_Endpoint_And_Get_Style_Content(BaseStyle style)
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
}