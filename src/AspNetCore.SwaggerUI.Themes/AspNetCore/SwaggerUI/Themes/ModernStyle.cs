namespace AspNetCore.SwaggerUI.Themes;

/// <summary>
/// Represents a modern style for Swagger UI.
/// </summary>
public sealed class ModernStyle : BaseStyle
{
    private ModernStyle(string fileName) : base(fileName)
    {
    }

    internal override ModernStyle Common => new("modern.common.css");

    /// <summary>
    /// Apply a modern light style to your Swagger UI.
    /// </summary>
    public static ModernStyle Light => new("modern.light.css");

    /// <summary>
    /// Apply a modern sleek dark style to your Swagger UI.
    /// </summary>
    public static ModernStyle Dark => new("modern.dark.css");

    /// <inheritdoc/>
    protected override string GetStyleName()
    {
#if NET6_0_OR_GREATER
        return char.ToUpper(FileName[0]) + FileName[1..(FileName.LastIndexOf('.'))].Replace('.', ' ');
#else
        return char.ToUpper(FileName[0]) + FileName.Substring(1, FileName.LastIndexOf('.')).Replace('.', ' ');
#endif
    }
}