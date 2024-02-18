namespace AspNetCore.SwaggerUI.Themes;

/// <summary>
/// Represents a style for Swagger UI.
/// </summary>
public sealed class Style : BaseStyle
{
    private Style(string fileName) : base(fileName)
    {
    }

    internal override Style Common => new("common.css");

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
}