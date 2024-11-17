namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents the base class used to create a style for Swagger UI.
/// </summary>
public abstract class BaseStyle
{
    protected BaseStyle(string fileName)
    {
        CheckFileNameExtension(fileName);
        FileName = fileName;
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
#if NET6_0_OR_GREATER
        return char.ToUpper(FileName[0]) + FileName[1..FileName.LastIndexOf('.')];
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