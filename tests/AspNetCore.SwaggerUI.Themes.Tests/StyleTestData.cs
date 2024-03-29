﻿namespace AspNetCore.SwaggerUI.Themes.Tests;

/// <summary>
/// List of all styles to test.
/// </summary>
public class StyleTestData : TheoryData<BaseStyle>
{
    public StyleTestData()
    {
        AddRange(
            Style.Dark,
            Style.Forest,
            Style.DeepSea,
            Style.Desert,
            ModernStyle.Light,
            ModernStyle.Dark,
            ModernStyle.Forest,
            ModernStyle.DeepSea,
            ModernStyle.Futuristic,
            ModernStyle.Desert
        );
    }
}