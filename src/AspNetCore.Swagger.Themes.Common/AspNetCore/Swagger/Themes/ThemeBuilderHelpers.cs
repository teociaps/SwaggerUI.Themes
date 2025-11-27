using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Shared logic for Swagger UI builder extensions.
/// </summary>
internal static class ThemeBuilderHelpers
{
    internal static string GetConfiguredJavaScript(IDictionary<string, object> additionalSettings) =>
        AdvancedOptions.Apply(FileProvider.GetResourceText(FileProvider.JsFilename), additionalSettings, MimeTypes.Text.Javascript);

    internal static void RegisterJavaScriptEndpoint(IApplicationBuilder application, string javascript) =>
        FileProvider.AddGetEndpoint(application, FileProvider.ScriptsPath + FileProvider.JsFilename, javascript, MimeTypes.Text.Javascript);

    internal static void ConfigureThemeWithSwitcher(
        IApplicationBuilder application,
        BaseTheme theme,
        IDictionary<string, object> additionalSettings,
        ThemeSwitcherOptions switcherOptions,
        Action<BaseTheme> loadAllThemesCallback)
    {
        ThemeSwitcher.Configure(application, theme, additionalSettings, switcherOptions);

        // Check if theme switcher is enabled
        if (!additionalSettings.TryGetValue(AdvancedOptions.ThemeSwitcher, out var enabled) || enabled is not true)
            return;

        switcherOptions ??= ThemeSwitcherOptions.All();

        // Load all available themes
        foreach (var availableTheme in ThemeSwitcher.GetAvailableThemes(theme, switcherOptions))
            loadAllThemesCallback(availableTheme);
    }

    internal static void ConfigureCustomThemeWithSwitcher(
        IApplicationBuilder application,
        string themeName,
        bool isStandalone,
        IDictionary<string, object> additionalSettings,
        ThemeSwitcherOptions switcherOptions)
    {
        if (isStandalone)
            return;

        if (!additionalSettings.TryGetValue(AdvancedOptions.ThemeSwitcher, out var enabled) || enabled is not true)
            return;

        switcherOptions ??= ThemeSwitcherOptions.All();

        var filteredThemes = ThemeSwitcher.GetFilteredThemes(switcherOptions).ToList();
        FileProvider.ExposeThemeMetadata(application, filteredThemes, themeName, switcherOptions.ThemeDisplayFormat);
        FileProvider.FreezeCollections();
    }

    internal static (string ThemeContent, string CommonContent, bool LoadJs, bool IsStandalone, string ThemeName) LoadAssemblyTheme(Assembly assembly, string cssFilename)
    {
        var themeContent = FileProvider.GetResourceText(cssFilename, assembly, out var commonContent, out var loadJs);
        var isStandalone = string.IsNullOrEmpty(commonContent);
        var themeName = ExtractThemeName(cssFilename);

        return (themeContent, commonContent, loadJs, isStandalone, themeName);
    }

    private static string ExtractThemeName(string cssFilename) =>
        Path.GetFileNameWithoutExtension(cssFilename)
            .Replace("standalone.", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".min", "", StringComparison.OrdinalIgnoreCase);
}