using Microsoft.AspNetCore.Builder;
using System.Collections.Frozen;
using System.Reflection;

namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Centralized manager for theme registration and switcher functionality.
/// Single source of truth for all theme operations.
/// </summary>
internal static class ThemeSwitcher
{
    private static readonly FrozenSet<BaseTheme> s_predefinedThemes = new[]
    {
        Theme.Dark, Theme.Light, Theme.Forest, Theme.DeepSea, Theme.Desert, Theme.Futuristic
    }.ToFrozenSet<BaseTheme>();

    private static readonly Dictionary<string, RegisteredTheme> s_registeredThemes = new(StringComparer.OrdinalIgnoreCase);
    private static FrozenDictionary<string, RegisteredTheme> s_frozenThemes;
    private static readonly HashSet<Type> s_scannedTypes = [];

    /// <summary>
    /// Configures theme switcher for a given theme and options.
    /// </summary>
    internal static void Configure(
        IApplicationBuilder application,
        BaseTheme theme,
        IDictionary<string, object> additionalSettings,
        ThemeSwitcherOptions switcherOptions = null)
    {
        RegisterTheme(theme, theme.FileName, isStandalone: false);

        if (!additionalSettings.TryGetValue(AdvancedOptions.ThemeSwitcher, out var enabled) || enabled is not true)
            return;

        switcherOptions ??= ThemeSwitcherOptions.All();

        if (switcherOptions.CustomThemeMode is CustomThemeMode.AutoDiscover)
            AutoDiscoverCustomThemes(theme);

        RegisterPredefinedThemes(switcherOptions);

        var filteredThemes = GetFilteredThemes(switcherOptions).ToList();

        if (!filteredThemes.Any(rt => rt.Name.Equals(theme.GetThemeName(), StringComparison.OrdinalIgnoreCase)))
        {
            var currentRegistered = s_registeredThemes.TryGetValue(theme.GetThemeName(), out var rt)
                ? rt
                : new RegisteredTheme(theme, FileProvider.StylesPath + theme.FileName, false);
            filteredThemes.Insert(0, currentRegistered);
        }

        // Validate with actual discovered themes
        switcherOptions.Validate(theme.GetThemeName(), filteredThemes);

        FileProvider.ExposeThemeMetadata(application, filteredThemes, theme.GetThemeName(), switcherOptions.ThemeDisplayFormat);
        FreezeCollections();
    }

    /// <summary>
    /// Registers a theme for switching.
    /// </summary>
    internal static void RegisterTheme(BaseTheme theme, string cssPath, bool isStandalone)
    {
        var themeName = theme.GetThemeName();

        if (s_registeredThemes.ContainsKey(themeName))
            return;

        s_registeredThemes[themeName] = new RegisteredTheme(
            theme,
            FileProvider.StylesPath + cssPath,
            isStandalone
        );
    }

    /// <summary>
    /// Registers a custom theme from assembly.
    /// </summary>
    internal static void RegisterCustomTheme(string cssFilename, bool isStandalone)
    {
        var themeName = ExtractThemeName(cssFilename);
        var cssPath = FileProvider.StylesPath + cssFilename;

        if (s_registeredThemes.ContainsKey(themeName))
            return;

        s_registeredThemes[themeName] = new RegisteredTheme(
            null,
            cssPath,
            isStandalone
        );
    }

    /// <summary>
    /// Registers theme endpoints for CSS files.
    /// </summary>
    internal static void RegisterThemeEndpoints(IApplicationBuilder application, BaseTheme theme, IDictionary<string, object> advancedOptions)
    {
        RegisterTheme(theme, theme.FileName, isStandalone: false);

        var commonContent = FileProvider.GetResourceText(theme.Common.FileName);
        commonContent = AdvancedOptions.Apply(commonContent, advancedOptions, MimeTypes.Text.Css);
        var commonPath = FileProvider.StylesPath + theme.Common.FileName;
        FileProvider.AddGetEndpoint(application, commonPath, commonContent);

        var themeContent = FileProvider.GetResourceText(theme.FileName, theme.GetType());
        var themePath = FileProvider.StylesPath + theme.FileName;
        FileProvider.AddGetEndpoint(application, themePath, themeContent);
    }

    /// <summary>
    /// Loads theme content (common + theme CSS combined).
    /// </summary>
    internal static string LoadThemeContent(BaseTheme theme, IDictionary<string, object> advancedOptions)
    {
        var commonCss = FileProvider.GetResourceText(theme.Common.FileName);
        commonCss = AdvancedOptions.Apply(commonCss, advancedOptions, MimeTypes.Text.Css);

        var themeCss = FileProvider.GetResourceText(theme.FileName, theme.GetType());

        return commonCss + Environment.NewLine + themeCss;
    }

    /// <summary>
    /// Creates a link tag for a theme.
    /// </summary>
    internal static string CreateThemeLink(BaseTheme theme, bool disabled = false)
    {
        var themePath = FileProvider.StylesPath + theme.FileName;
        var disabledAttr = disabled ? @" disabled=""disabled""" : "";
        return $@"<link rel=""stylesheet"" type=""text/css"" href=""{themePath}""{disabledAttr} data-theme=""{theme.GetThemeName()}"" />";
    }

    /// <summary>
    /// Creates a script that marks the current theme links.
    /// </summary>
    internal static string CreateCurrentThemeMarkerScript(BaseTheme currentTheme) =>
        $$"""
        <script>
        (function() {
            document.addEventListener('DOMContentLoaded', function() {
                const currentThemeLinks = document.querySelectorAll('link[rel="stylesheet"][href*="{{currentTheme.FileName}}"]');
                currentThemeLinks.forEach(link => {
                    if (!link.hasAttribute('data-theme')) {
                        link.setAttribute('data-theme', '{{currentTheme.GetThemeName()}}');
                    }
                });
            });
        })();
        </script>
        """;

    /// <summary>
    /// Auto-discovers all custom theme properties from the current theme's type and related types.
    /// </summary>
    private static void AutoDiscoverCustomThemes(BaseTheme currentTheme)
    {
        var currentType = currentTheme.GetType();
        var currentAssembly = currentType.Assembly;

        // Scan the current theme's assembly
        ScanAssemblyForThemes(currentAssembly);

        // Also scan the entry assembly if it's different
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is not null && entryAssembly != currentAssembly)
        {
            ScanAssemblyForThemes(entryAssembly);
        }
    }

    /// <summary>
    /// Scans an assembly for custom theme types.
    /// </summary>
    private static void ScanAssemblyForThemes(Assembly assembly)
    {
        var themeTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseTheme)) && t != typeof(Theme))
            .ToList();

        foreach (var type in themeTypes)
            ScanTypeForThemes(type);
    }

    /// <summary>
    /// Scans a type for static properties returning BaseTheme and registers them.
    /// </summary>
    private static void ScanTypeForThemes(Type type)
    {
        if (!s_scannedTypes.Add(type))
            return;

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => typeof(BaseTheme).IsAssignableFrom(p.PropertyType))
            .ToList();

        foreach (var property in properties)
        {
            try
            {
                if (property.GetValue(null) is not BaseTheme themeInstance)
                    continue;

                if (IsStandaloneTheme(themeInstance))
                    continue;

                RegisterTheme(themeInstance, themeInstance.FileName, isStandalone: false);
            }
            catch
            {
                // Silently skip properties that can't be accessed
            }
        }
    }

    /// <summary>
    /// Determines if a theme is standalone by checking its filename.
    /// </summary>
    private static bool IsStandaloneTheme(BaseTheme theme) =>
        theme.FileName.Contains("standalone", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets available themes (excluding current theme).
    /// </summary>
    internal static IEnumerable<BaseTheme> GetAvailableThemes(BaseTheme currentTheme, ThemeSwitcherOptions options)
    {
        var currentThemeName = currentTheme.GetThemeName();
        return GetFilteredThemes(options)
            .Where(rt => !rt.Name.Equals(currentThemeName, StringComparison.OrdinalIgnoreCase))
            .Select(rt => rt.Theme!)
            .Where(t => t is not null);
    }

    /// <summary>
    /// Gets filtered themes based on options.
    /// </summary>
    internal static IEnumerable<RegisteredTheme> GetFilteredThemes(ThemeSwitcherOptions options)
    {
        var themes = s_frozenThemes?.Values ?? s_registeredThemes.ToFrozenDictionary().Values;

        return themes
            .Where(rt => !rt.IsStandalone)
            .Where(rt => ShouldIncludeTheme(rt, options));
    }

    /// <summary>
    /// Determines if a theme is predefined by checking its name.
    /// </summary>
    internal static bool IsPredefinedTheme(BaseTheme theme) =>
        FileProvider.IsPredefinedTheme(theme.GetThemeName());

    /// <summary>
    /// Determines if a theme should be included based on options.
    /// </summary>
    private static bool ShouldIncludeTheme(RegisteredTheme registeredTheme, ThemeSwitcherOptions options)
    {
        var isPredefined = FileProvider.IsPredefinedTheme(registeredTheme.Name);

        if (options.ExcludedThemes.Any(t => t.GetThemeName().Equals(registeredTheme.Name, StringComparison.OrdinalIgnoreCase)))
            return false;

        if (isPredefined)
        {
            return options.IncludeAllPredefinedThemes ||
                   options.IncludedThemes.Any(t => t.GetThemeName().Equals(registeredTheme.Name, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            if (options.CustomThemeMode is CustomThemeMode.None)
                return false;

            if (!options.IncludeAllPredefinedThemes && options.IncludedThemes.Count > 0)
                return options.IncludedThemes.Any(t => t.GetThemeName().Equals(registeredTheme.Name, StringComparison.OrdinalIgnoreCase));

            return true;
        }
    }

    /// <summary>
    /// Registers all predefined themes based on options.
    /// </summary>
    private static void RegisterPredefinedThemes(ThemeSwitcherOptions options)
    {
        foreach (var theme in s_predefinedThemes)
        {
            var registered = new RegisteredTheme(theme, FileProvider.StylesPath + theme.FileName, false);
            if (ShouldIncludeTheme(registered, options))
                RegisterTheme(theme, theme.FileName, isStandalone: false);
        }
    }

    /// <summary>
    /// Freezes collections after startup for better read performance.
    /// </summary>
    private static void FreezeCollections()
    {
        if (s_frozenThemes == null && s_registeredThemes.Count > 0)
            s_frozenThemes = s_registeredThemes.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Extracts theme name from CSS filename.
    /// </summary>
    private static string ExtractThemeName(string cssFilename) =>
        Path.GetFileNameWithoutExtension(cssFilename)
            .Replace("standalone.", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".min", "", StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// Represents a registered theme with metadata.
/// </summary>
internal record RegisteredTheme(BaseTheme Theme, string CssPath, bool IsStandalone)
{
    public string Name => Theme?.GetThemeName() ??
                         Path.GetFileNameWithoutExtension(CssPath)
                             .Replace("standalone.", "", StringComparison.OrdinalIgnoreCase)
                             .Replace(".min", "", StringComparison.OrdinalIgnoreCase);
}