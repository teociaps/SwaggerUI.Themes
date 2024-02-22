﻿namespace AspNetCore.SwaggerUI.Themes.Tests;

/// <summary>
/// List of styles to test.
/// </summary>
public class StyleTestData : IEnumerable<object[]> // object[] because [ClassData]
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [Style.Dark];
        yield return [Style.Forest];
        yield return [Style.DeepSea];
        yield return [ModernStyle.Light];
        yield return [ModernStyle.Dark];
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
