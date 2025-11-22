namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Configuration options for the theme switcher feature.
/// </summary>
public class ThemeSwitcherOptions
{
    /// <summary>
    /// <para>Gets or sets whether to include all predefined themes in the switcher.</para>
    /// <para>Default is <see langword="true"/>.</para>
    /// </summary>
    public bool IncludeAllPredefinedThemes { get; set; } = true;

    /// <summary>
    /// Gets specific themes to include (both predefined and custom).
    /// </summary>
    public List<BaseTheme> IncludedThemes { get; } = [];

    /// <summary>
    /// Gets themes to exclude from the switcher (both predefined and custom).
    /// </summary>
    public List<BaseTheme> ExcludedThemes { get; } = [];

    /// <summary>
    /// <para>Gets or sets the custom theme inclusion strategy.</para>
    /// <para>Default is <see cref="CustomThemeMode.AutoDiscover"/>.</para>
    /// </summary>
    public CustomThemeMode CustomThemeMode { get; set; } = CustomThemeMode.AutoDiscover;

    /// <summary>
    /// <para>Gets or sets the display name format for themes in the dropdown.</para>
    /// <para>Available placeholders: {name} (e.g., "Theme: {name}" becomes "Theme: Dark").</para>
    /// <para>Default is <c>"{name}"</c>.</para>
    /// </summary>
    public string ThemeDisplayFormat { get; set; } = "{name}";

    /// <summary>
    /// Validates the configuration with actual discovered themes.
    /// </summary>
    /// <param name="defaultTheme">The default theme name.</param>
    /// <param name="discoveredThemes">The list of themes discovered and filtered. Pass <see langword="null"/> to skip runtime validation.</param>
    internal void Validate(string defaultTheme, IList<RegisteredTheme> discoveredThemes = null)
    {
        // If discovered themes provided, validate with actual runtime data
        if (discoveredThemes is not null)
        {
            if (discoveredThemes.Count < 2)
            {
                throw new InvalidOperationException(
                    "Theme switcher requires at least 2 themes to be available. " +
                    $"Only {discoveredThemes.Count} theme(s) were found after auto-discovery and filtering. " +
                    "Ensure you have enough themes available or adjust your ThemeSwitcherOptions configuration.");
            }

            // Ensure default theme is in the discovered list
            if (!string.IsNullOrEmpty(defaultTheme) &&
                !discoveredThemes.Any(t => t.Name.Equals(defaultTheme, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    $"Default theme '{defaultTheme}' was not found in the discovered themes. " +
                    "Ensure the default theme is included in your configuration.");
            }

            return; // Runtime validation complete
        }

        // Static validation (without runtime discovery data)
        // Calculate available predefined themes
        var predefinedCount = IncludeAllPredefinedThemes
            ? 6 - ExcludedThemes.Count(ThemeSwitcher.IsPredefinedTheme)
            : IncludedThemes.Count(ThemeSwitcher.IsPredefinedTheme);

        // Calculate available custom themes (only for non-AutoDiscover modes)
        var customCount = 0;
        if (CustomThemeMode is CustomThemeMode.ExplicitOnly)
        {
            customCount = IncludedThemes.Count(t => !ThemeSwitcher.IsPredefinedTheme(t));
        }
        // Note: For AutoDiscover mode, we can't validate custom theme count here
        // as discovery happens later at runtime

        var availableCount = predefinedCount + customCount;

        // Only validate minimum theme count for non-AutoDiscover modes
        if (CustomThemeMode is not CustomThemeMode.AutoDiscover && availableCount < 2)
        {
            throw new InvalidOperationException(
                "Theme switcher requires at least 2 themes to be available. " +
                "Current configuration would result in fewer than 2 themes.");
        }

        // For AutoDiscover mode with no predefined themes, just warn that it relies on discovery
        if (CustomThemeMode is CustomThemeMode.AutoDiscover && predefinedCount == 0)
        {
            // This is valid but relies on auto-discovery finding at least 2 custom themes
            // We can't validate this here, so we allow it to proceed
        }

        // Ensure default theme is included
        if (string.IsNullOrEmpty(defaultTheme)) return;

        var isCustomTheme = !FileProvider.IsPredefinedTheme(defaultTheme);

        if (!IncludeAllPredefinedThemes &&
            !IncludedThemes.Any(t => t.GetThemeName().Equals(defaultTheme, StringComparison.OrdinalIgnoreCase)))
        {
            if (!isCustomTheme || CustomThemeMode is CustomThemeMode.None)
            {
                throw new InvalidOperationException(
                    $"Default theme '{defaultTheme}' must be included in the theme switcher.");
            }
            // For custom themes with AutoDiscover, allow it (will be validated at runtime)
        }

        if (ExcludedThemes.Any(t => t.GetThemeName().Equals(defaultTheme, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"Default theme '{defaultTheme}' cannot be excluded from the theme switcher.");
        }
    }

    /// <summary>
    /// Includes specific themes in the switcher (both predefined and custom).
    /// This sets <see cref="IncludeAllPredefinedThemes"/> to false and clears <see cref="IncludedThemes"/>.
    /// </summary>
    /// <param name="themes">The themes to include.</param>
    /// <returns>The current options instance for method chaining.</returns>
    public ThemeSwitcherOptions WithThemes(params BaseTheme[] themes)
    {
        IncludeAllPredefinedThemes = false;
        IncludedThemes.Clear();
        IncludedThemes.AddRange(themes);
        return this;
    }

    /// <summary>
    /// Excludes specific themes from the switcher (both predefined and custom).
    /// </summary>
    /// <param name="themes">The themes to exclude.</param>
    /// <returns>The current options instance for method chaining.</returns>
    public ThemeSwitcherOptions ExcludeThemes(params BaseTheme[] themes)
    {
        ExcludedThemes.Clear();
        ExcludedThemes.AddRange(themes);
        return this;
    }

    /// <summary>
    /// Sets whether to include all predefined themes.
    /// </summary>
    /// <param name="include"><see langword="true"/> to include all predefined themes, <see langword="false"/> otherwise.</param>
    /// <returns>The current options instance for method chaining.</returns>
    public ThemeSwitcherOptions WithAllPredefinedThemes(bool include = true)
    {
        IncludeAllPredefinedThemes = include;
        return this;
    }

    /// <summary>
    /// Sets whether to include custom themes.
    /// </summary>
    /// <param name="mode">The custom theme inclusion mode.</param>
    /// <returns>The current options instance for method chaining.</returns>
    public ThemeSwitcherOptions WithCustomThemes(CustomThemeMode mode)
    {
        CustomThemeMode = mode;
        return this;
    }

    /// <summary>
    /// Sets the display format for theme names.
    /// </summary>
    /// <param name="format">The format string (use <c>{name}</c> as placeholder).</param>
    /// <returns>The current options instance for method chaining.</returns>
    public ThemeSwitcherOptions WithDisplayFormat(string format)
    {
        ThemeDisplayFormat = format ?? "{name}";
        return this;
    }

    /// <summary>
    /// Creates a configuration that includes all themes (predefined + custom with auto-discovery).
    /// </summary>
    public static ThemeSwitcherOptions All() => new();

    /// <summary>
    /// Creates a configuration that includes only predefined themes.
    /// </summary>
    public static ThemeSwitcherOptions PredefinedOnly() => new()
    {
        IncludeAllPredefinedThemes = true,
        CustomThemeMode = CustomThemeMode.None
    };

    /// <summary>
    /// Creates a configuration that includes only custom themes with auto-discovery.
    /// </summary>
    public static ThemeSwitcherOptions CustomOnly() => new()
    {
        IncludeAllPredefinedThemes = false,
        CustomThemeMode = CustomThemeMode.AutoDiscover
    };
}

/// <summary>
/// Defines how custom themes should be handled in the theme switcher.
/// </summary>
public enum CustomThemeMode
{
    /// <summary>
    /// Do not include any custom themes.
    /// </summary>
    None = 0,

    /// <summary>
    /// Only include explicitly registered custom themes (no auto-discovery).
    /// </summary>
    ExplicitOnly = 1,

    /// <summary>
    /// Auto-discover and include all custom theme properties from custom theme classes.
    /// </summary>
    AutoDiscover = 2
}