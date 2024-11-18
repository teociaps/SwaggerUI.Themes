namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a style for Swagger UI.
/// </summary>
public class Style : BaseStyle
{
    protected Style(string fileName) : base(fileName)
    {
    }

    internal override Style Common => new("common.css");

    /// <summary>
    /// Apply a light style to your Swagger UI.
    /// </summary>
    public static Style Light => new("light.css");

    /// <summary>
    /// Apply a dark style to your Swagger UI.
    /// </summary>
    public static Style Dark => new("dark.css");

    /// <summary>
    /// Apply a forest tones style to your Swagger UI.
    /// </summary>
    public static Style Forest => new("forest.css");

    /// <summary>
    /// Apply a deep sea tones style to your Swagger UI.
    /// </summary>
    public static Style DeepSea => new("deepsea.css");

    /// <summary>
    /// Apply a desert tones style to your Swagger UI.
    /// </summary>
    public static Style Desert => new("desert.css");
}