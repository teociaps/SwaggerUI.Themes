namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents the base class used to create a style for Swagger UI.
/// </summary>
public abstract class BaseStyle
{
    /// <summary>
    /// Initializes a new instance with the specified CSS file name and an option to use the minified version.
    /// </summary>
    /// <remarks>
    /// If <paramref name="useMinified"/> is <see langword="true"/> and the provided file name ends with ".css", the constructor will use
    /// the corresponding ".min.css" file. If useMinified is false, the non-minified ".css" file is used regardless of
    /// the input extension.
    /// </remarks>
    /// <param name="fileName">The name of the CSS file to use. Must include the ".css" or ".min.css" extension.</param>
    /// <param name="useMinified"><see langword="true"/> to use the minified ".min.css" version of the file; otherwise, <see langword="false"/>.</param>
    protected BaseStyle(string fileName, bool useMinified = true)
    {
        CheckFileNameExtension(fileName);

        // Store the base filename without .min.css extension
        var baseFileName = fileName.Replace(".min.css", ".css", StringComparison.OrdinalIgnoreCase);

        // Determine actual filename based on useMinified flag
        FileName = useMinified
            ? baseFileName.Replace(".css", ".min.css", StringComparison.OrdinalIgnoreCase)
            : baseFileName;
    }

    internal virtual BaseStyle Common { get; }

    internal virtual bool LoadAdditionalJs => false;

    /// <summary>
    /// Gets the file name associated with the selected style.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Returns the file name as a string representation of the style.
    /// </summary>
    /// <returns>The file name associated with the style.</returns>
    public override string ToString() => $"{GetStyleName()} Style";

    /// <summary>
    /// Gets the name of the style without the file extension.
    /// </summary>
    /// <returns>The style name.</returns>
    protected virtual string GetStyleName()
    {
        var nameWithoutExtension = FileName
            .Replace(".min.css", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".css", "", StringComparison.OrdinalIgnoreCase);

        return char.ToUpper(nameWithoutExtension[0]) + nameWithoutExtension[1..];
    }

    private static void CheckFileNameExtension(string fileName)
    {
        if (!fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase) &&
            !fileName.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The file name extension doesn't match the CSS style format!", nameof(fileName));
        }
    }
}