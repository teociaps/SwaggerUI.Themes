namespace AspNetCore.SwaggerUI.Themes;

/// <summary>
/// Represents a style for Swagger UI.
/// </summary>
public sealed class Style
{
    private Style(string fileName, StyleFormat styleFormat = StyleFormat.Css)
    {
        Format = styleFormat;
        CheckFileNameExtension(fileName);
        FileName = fileName;
    }

    /// <summary>
    /// Gets the file name associated with the selected style.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets the format of the style (e.g., Css, Scss, etc.).
    /// </summary>
    public StyleFormat Format { get; }

    /// <summary>
    /// Gets an instance of the style class representing the dark style.
    /// </summary>
    public static Style Dark => new("dark.css");

    // TODO: custom style

    /// <summary>
    /// Returns the file name and format as a string representation of the style.
    /// </summary>
    /// <returns>The file name and format associated with the style.</returns>
    public override string ToString() => $"{GetStyleName()} Style ({Format})";

    /// <summary>
    /// Gets the name of the style without the file extension.
    /// </summary>
    /// <returns>The style name.</returns>
    public string GetStyleName()
    {
        return char.ToUpper(FileName[0]) + FileName[1..(FileName.LastIndexOf('.'))];
    }

    internal string FormatText => Format.ToString().ToLowerInvariant();

    private void CheckFileNameExtension(string fileName)
    {
        if (!fileName.EndsWith($".{FormatText}", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"The file name extension doesn't match the chosen style format: {FormatText}.", nameof(fileName));
    }
}

/// <summary>
/// Represents the format of a style file.
/// </summary>
public enum StyleFormat
{
    /// <summary>
    /// Cascading Style Sheets format.
    /// </summary>
    Css,

    /// <summary>
    /// Sassy CSS (SCSS) format.
    /// </summary>
    Scss,

    /// <summary>
    /// Syntactically Awesome Style Sheets (Sass) format.
    /// </summary>
    Sass
}