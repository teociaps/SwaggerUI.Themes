namespace AspNetCore.SwaggerUI.Themes.Tests;

/// <summary>
/// List of styles to test.
/// </summary>
public class StyleTestData : IEnumerable<object[]> // object[] because [ClassData]
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [Style.Common];
        yield return [Style.Dark];
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
