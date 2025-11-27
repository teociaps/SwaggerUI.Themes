namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents the base class used to create a theme for Swagger UI.
/// </summary>
public abstract class BaseTheme
{
    /// <summary>
    /// Initializes a new instance with the specified CSS file name and an option to use the minified version.
    /// </summary>
    /// <remarks>
    /// If <paramref name="useMinified"/> is <see langword="true"/> and the provided file name ends with ".css", the constructor will use
    /// the corresponding ".min.css" file. If useMinified is <see langword="false"/>, the non-minified ".css" file is used regardless of
    /// the input extension.
    /// </remarks>
    /// <param name="fileName">The name of the CSS file to use. Must include the ".css" or ".min.css" extension.</param>
    /// <param name="useMinified"><see langword="true"/> to use the minified ".min.css" version of the file; otherwise, <see langword="false"/>.</param>
    protected BaseTheme(string fileName, bool useMinified = false)
    {
        CheckFileNameExtension(fileName);

        // Store the base filename without .min.css extension
        var baseFileName = fileName.Replace(".min.css", ".css", StringComparison.OrdinalIgnoreCase);

        // Determine actual filename based on useMinified flag
        FileName = useMinified
            ? baseFileName.Replace(".css", ".min.css", StringComparison.OrdinalIgnoreCase)
            : baseFileName;
    }

    internal virtual BaseTheme Common { get; }

    internal virtual bool LoadAdditionalJs => false;

    /// <summary>
    /// Gets the file name associated with the selected theme.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Returns the file name as a string representation of the theme.
    /// </summary>
    /// <returns>The file name associated with the theme.</returns>
    public override string ToString() => $"{GetThemeName()} Theme";

    /// <summary>
    /// Gets the name of the theme without the file extension.
    /// </summary>
    /// <returns>The theme name.</returns>
    protected internal virtual string GetThemeName()
    {
        var fileName = FileName;

        // Remove extension
        if (fileName.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase))
            fileName = fileName[..^8];
        else if (fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            fileName = fileName[..^4];

        if (string.IsNullOrEmpty(fileName))
            return string.Empty;

        return char.ToUpper(fileName[0]) + fileName[1..];
    }

    private static void CheckFileNameExtension(string fileName)
    {
        if (!fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase) &&
            !fileName.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The file name extension doesn't match the CSS theme format!", nameof(fileName));
        }
    }
}