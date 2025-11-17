namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a theme for Swagger UI.
/// </summary>
public class Theme : BaseTheme
{
    /// <inheritdoc/>
    protected Theme(string fileName, bool useMinified = false) : base(fileName, useMinified)
    {
    }

    internal override Theme Common => new("common.css", true);

    internal override bool LoadAdditionalJs => true;

    /// <summary>
    /// Apply a light theme to your Swagger UI.
    /// </summary>
    public static Theme Light => new("light.css", true);

    /// <summary>
    /// Apply a sleek dark theme to your Swagger UI.
    /// </summary>
    public static Theme Dark => new("dark.css", true);

    /// <summary>
    /// Apply a forest tones theme to your Swagger UI.
    /// </summary>
    public static Theme Forest => new("forest.css", true);

    /// <summary>
    /// Apply a deep sea tones theme to your Swagger UI.
    /// </summary>
    public static Theme DeepSea => new("deepsea.css", true);

    /// <summary>
    /// Apply a futuristic theme to your Swagger UI.
    /// </summary>
    public static Theme Futuristic => new("futuristic.css", true);

    /// <summary>
    /// Apply a desert tones theme to your Swagger UI.
    /// </summary>
    public static Theme Desert => new("desert.css", true);

    /// <inheritdoc/>
    protected override string GetThemeName()
    {
        var nameWithoutExtension = FileName
            .Replace(".min.css", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".css", "", StringComparison.OrdinalIgnoreCase);

        return char.ToUpper(nameWithoutExtension[0]) + nameWithoutExtension[1..];
    }
}