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
            ModernStyle.Light,
            ModernStyle.Dark,
            ModernStyle.Forest,
            ModernStyle.DeepSea,
            ModernStyle.Futuristic,
            ModernStyle.Desert,
            CustomStyle.Custom,
            CustomModernStyle.CustomModern
        );
    }
}