namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a modern style for Swagger UI.
/// </summary>
public class ModernStyle : BaseStyle
{
    /// <inheritdoc/>
    protected ModernStyle(string fileName, bool useMinified = false) : base(fileName, useMinified)
    {
    }

    internal override ModernStyle Common => new("modern.common.css", true);

    internal override bool LoadAdditionalJs => true;

    /// <summary>
    /// Apply a modern light style to your Swagger UI.
    /// </summary>
    public static ModernStyle Light => new("modern.light.css", true);

    /// <summary>
    /// Apply a modern sleek dark style to your Swagger UI.
    /// </summary>
    public static ModernStyle Dark => new("modern.dark.css", true);

    /// <summary>
    /// Apply a modern forest tones style to your Swagger UI.
    /// </summary>
    public static ModernStyle Forest => new("modern.forest.css", true);

    /// <summary>
    /// Apply a modern deep sea tones style to your Swagger UI.
    /// </summary>
    public static ModernStyle DeepSea => new("modern.deepsea.css", true);

    /// <summary>
    /// Apply a modern futuristic style to your Swagger UI.
    /// </summary>
    public static ModernStyle Futuristic => new("modern.futuristic.css", true);

    /// <summary>
    /// Apply a modern desert tones style to your Swagger UI.
    /// </summary>
    public static ModernStyle Desert => new("modern.desert.css", true);

    /// <inheritdoc/>
    protected override string GetStyleName()
    {
        var nameWithoutExtension = FileName
            .Replace(".min.css", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".css", "", StringComparison.OrdinalIgnoreCase);

        return char.ToUpper(nameWithoutExtension[0]) + nameWithoutExtension[1..].Replace('.', ' ');
    }
}