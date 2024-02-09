namespace AspNetCore.SwaggerUI.Themes;

/// <summary>
/// Represents a style for Swagger UI.
/// </summary>
public sealed class Style
{
    private Style(string fileName)
    {
        CheckFileNameExtension(fileName);
        FileName = fileName;
    }

    /// <summary>
    /// Gets the file name associated with the selected style.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Apply a default dark style to your Swagger UI.
    /// </summary>
    public static Style Dark => new("dark.css");

    internal static Style Common => new("common.css");

    // TODO: custom style

    // TODO: default and modern styles

    /// <summary>
    /// Returns the file name and format as a string representation of the style.
    /// </summary>
    /// <returns>The file name and format associated with the style.</returns>
    public override string ToString() => $"{GetStyleName()} Style";

    /// <summary>
    /// Gets the name of the style without the file extension.
    /// </summary>
    /// <returns>The style name.</returns>
    public string GetStyleName()
    {
    #if NET6_0_OR_GREATER
        return char.ToUpper(FileName[0]) + FileName[1..(FileName.LastIndexOf('.'))];
    #else
        return char.ToUpper(FileName[0]) + FileName.Substring(1, FileName.LastIndexOf('.'));
    #endif
    }

    private static void CheckFileNameExtension(string fileName)
    {
        if (!fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The file name extension doesn't match the CSS style format!", nameof(fileName));
    }
}