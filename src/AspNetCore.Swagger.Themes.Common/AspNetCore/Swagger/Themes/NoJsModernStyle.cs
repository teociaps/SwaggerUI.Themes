namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a modern style for Swagger UI without additional JavaScript.
/// </summary>
public class NoJsModernStyle : ModernStyle
{
    protected NoJsModernStyle(string fileName) : base(fileName)
    {
    }

    /// <summary>
    /// Indicates whether additional JavaScript should be loaded and new functionalities will be added to the UI.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// See the <see cref="ModernStyle"/> class for JS version.
    /// </remarks>
    public override bool LoadAdditionalJs => false;
}