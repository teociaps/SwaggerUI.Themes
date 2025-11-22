using AspNetCore.Swagger.Themes;
using NSwag.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="SwaggerUiSettings"/>.
/// </summary>
public static class SwaggerUiSettingsExtensions
{
    private static ThemeSwitcherOptions s_switcherOptions;

    extension(SwaggerUiSettings settings)
    {
        /// <summary>
        /// Enables all the advanced options:
        /// <list type="bullet">
        /// <item>
        /// Pinnable topbar:
        /// <see cref="EnablePinnableTopbar"/>
        /// </item>
        /// <item>
        /// Back to top button:
        /// <see cref="ShowBackToTopButton"/>
        /// </item>
        /// <item>
        /// Sticky operations:
        /// <see cref="EnableStickyOperations"/>
        /// </item>
        /// <item>
        /// Expand or collapse all operations:
        /// <see cref="EnableExpandOrCollapseAllOperations"/>
        /// </item>
        /// <item>
        /// Theme switcher:
        /// <see cref="EnableThemeSwitcher"/>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="switcherOptions">
        /// Configuration options for the theme switcher.
        /// If <see langword="null"/>, all themes (predefined and custom) will be available.
        /// Use <see cref="ThemeSwitcherOptions"/> factory methods for common scenarios.
        /// </param>
        public void EnableAllAdvancedOptions(ThemeSwitcherOptions switcherOptions = null)
        {
            settings.EnablePinnableTopbar();
            settings.ShowBackToTopButton();
            settings.EnableStickyOperations();
            settings.EnableExpandOrCollapseAllOperations();
            settings.EnableThemeSwitcher(switcherOptions);
        }

        /// <summary>
        /// Enables the pinnable topbar feature.
        /// </summary>
        public void EnablePinnableTopbar() =>
            settings.AdditionalSettings.EnablePinnableTopbar();

        /// <summary>
        /// Shows a button to scroll back to the top of the page.
        /// </summary>
        public void ShowBackToTopButton() =>
            settings.AdditionalSettings.EnableBackToTop();

        /// <summary>
        /// Enables sticky operations.
        /// </summary>
        public void EnableStickyOperations() =>
            settings.AdditionalSettings.EnableStickyOperations();

        /// <summary>
        /// Enables the expand or collapse functionality for all operations inside a tag.
        /// </summary>
        public void EnableExpandOrCollapseAllOperations() =>
            settings.AdditionalSettings.EnableExpandOrCollapseAllOperations();

        /// <summary>
        /// Enables the theme switcher that allows users to change themes at runtime.
        /// The selected theme is persisted in browser local storage.
        /// </summary>
        /// <remarks>
        /// Note: not available for standalone themes as they don't include JavaScript.
        /// </remarks>
        /// <param name="switcherOptions">
        /// Configuration options for the theme switcher.
        /// If null, all themes (predefined and custom) will be available.
        /// Use <see cref="ThemeSwitcherOptions"/> factory methods for common scenarios.
        /// </param>
        public void EnableThemeSwitcher(ThemeSwitcherOptions switcherOptions = null)
        {
            settings.AdditionalSettings.EnableThemeSwitcher();

            if (switcherOptions is not null)
                s_switcherOptions = switcherOptions;
        }

        /// <summary>
        /// Gets the theme switcher options.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "extension method")]
        internal ThemeSwitcherOptions GetThemeSwitcherOptions() => s_switcherOptions;
    }
}