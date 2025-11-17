namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a style for Swagger UI.
/// </summary>
public class Style : BaseStyle
{
    /// <inheritdoc/>
    protected Style(string fileName, bool useMinified = false) : base(fileName, useMinified)
    {
    }

    internal override Style Common => new("common.css", true);

    internal override bool LoadAdditionalJs => true;

    /// <summary>
    /// Apply a light style to your Swagger UI.
    /// </summary>
    public static Style Light => new("light.css", true);

    /// <summary>
    /// Apply a sleek dark style to your Swagger UI.
    /// </summary>
    public static Style Dark => new("dark.css", true);

    /// <summary>
    /// Apply a forest tones style to your Swagger UI.
    /// </summary>
    public static Style Forest => new("forest.css", true);

    /// <summary>
    /// Apply a deep sea tones style to your Swagger UI.
    /// </summary>
    public static Style DeepSea => new("deepsea.css", true);

    /// <summary>
    /// Apply a futuristic style to your Swagger UI.
    /// </summary>
    public static Style Futuristic => new("futuristic.css", true);

    /// <summary>
    /// Apply a desert tones style to your Swagger UI.
    /// </summary>
    public static Style Desert => new("desert.css", true);

    /// <inheritdoc/>
    protected override string GetStyleName()
    {
        var nameWithoutExtension = FileName
            .Replace(".min.css", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".css", "", StringComparison.OrdinalIgnoreCase);

        return char.ToUpper(nameWithoutExtension[0]) + nameWithoutExtension[1..];
    }
}