namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a modern style for Swagger UI.
/// </summary>
public class ModernStyle : BaseStyle
{
    protected ModernStyle(string fileName) : base(fileName)
    {
    }

    internal override ModernStyle Common => new("modern.common.css");

    internal override bool LoadAdditionalJs => true;

    /// <summary>
    /// Apply a modern light style to your Swagger UI.
    /// </summary>
    public static ModernStyle Light => new("modern.light.css");

    /// <summary>
    /// Apply a modern sleek dark style to your Swagger UI.
    /// </summary>
    public static ModernStyle Dark => new("modern.dark.css");

    /// <summary>
    /// Apply a modern forest tones style to your Swagger UI.
    /// </summary>
    public static ModernStyle Forest => new("modern.forest.css");

    /// <summary>
    /// Apply a modern deep sea tones style to your Swagger UI.
    /// </summary>
    public static ModernStyle DeepSea => new("modern.deepsea.css");

    /// <summary>
    /// Apply a modern futuristic style to your Swagger UI.
    /// </summary>
    public static ModernStyle Futuristic => new("modern.futuristic.css");

    /// <summary>
    /// Apply a modern desert tones style to your Swagger UI.
    /// </summary>
    public static ModernStyle Desert => new("modern.desert.css");

    /// <inheritdoc/>
    protected override string GetStyleName()
    {
#if NET6_0_OR_GREATER
        return char.ToUpper(FileName[0]) + FileName[1..FileName.LastIndexOf('.')].Replace('.', ' ');
#else
        return char.ToUpper(FileName[0]) + FileName.Substring(1, FileName.LastIndexOf('.')).Replace('.', ' ');
#endif
    }
}