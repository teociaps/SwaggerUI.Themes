using Shouldly;
using static AspNetCore.SwaggerUI.Themes.StyleProvider;

namespace AspNetCore.SwaggerUI.Themes.Tests;

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
    public void Should_Embed_And_Retrieve_Style_From_ExecutingAssembly(Style style)
    {
        // Arrange/Act
        var styleText = GetResourceText(style.FileName);

        // Assert
        styleText.ShouldStartWith($"""
            /*
                {style}
            */

            @charset "UTF-8";
            """);
    }

    [Fact]
    public void Should_Throw_FileNotFoundException_When_External_Style()
    {
        // Arrange
        const string ExternalFileName = "style.css";

        // Act/Assert
        Should.Throw(() => GetResourceText(ExternalFileName), typeof(FileNotFoundException));
    }

    [Theory]
    [ClassData(typeof(StyleTestData))]
    public async Task Should_Add_Endpoint_And_Get_Style_Content(Style style)
    {
        // Arrange
        var fullPath = StylePath + style.FileName;
        var styleText = GetResourceText(style.FileName);

        // Act
        var response = await _styleProviderWebApplicationFactory.Client.GetAsync(fullPath);

        // Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBeEquivalentTo(styleText);
    }
}