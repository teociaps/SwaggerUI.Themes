namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Represents a dynamically created theme from a CSS file loaded at runtime.
/// Used when loading themes via Assembly.GetExecutingAssembly() to enable theme switcher functionality.
/// </summary>
/// <remarks>
/// Creates a new dynamic theme instance.
/// </remarks>
/// <param name="fileName">The CSS filename.</param>
/// <param name="isStandalone">Whether this is a standalone theme.</param>
internal sealed class DynamicTheme(string fileName, bool isStandalone, bool useMinified = false) : BaseTheme(fileName, useMinified)
{
    private readonly bool _isStandalone = isStandalone;

    internal override BaseTheme Common => _isStandalone ? this : new("common.css", false, true);

    internal override bool LoadAdditionalJs => !_isStandalone;
}