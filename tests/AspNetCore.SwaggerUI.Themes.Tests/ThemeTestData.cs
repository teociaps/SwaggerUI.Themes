using AspNetCore.Swagger.Themes.Tests.Styles;

namespace AspNetCore.Swagger.Themes.Tests;

/// <summary>
/// List of all styles to test.
/// </summary>
public class ThemeTestData : TheoryData<BaseTheme>
{
    public ThemeTestData()
    {
        AddRange(
            Theme.Light,
            Theme.Dark,
            Theme.Forest,
            Theme.DeepSea,
            Theme.Desert,
            Theme.Futuristic,
            CustomTheme.Custom
        );
    }
}