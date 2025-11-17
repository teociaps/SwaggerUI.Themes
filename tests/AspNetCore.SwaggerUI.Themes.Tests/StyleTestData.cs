using AspNetCore.Swagger.Themes.Tests.Styles;

namespace AspNetCore.Swagger.Themes.Tests;

/// <summary>
/// List of all styles to test.
/// </summary>
public class StyleTestData : TheoryData<BaseStyle>
{
    public StyleTestData()
    {
        AddRange(
            Style.Light,
            Style.Dark,
            Style.Forest,
            Style.DeepSea,
            Style.Desert,
            Style.Futuristic,
            CustomStyle.Custom
        );
    }
}