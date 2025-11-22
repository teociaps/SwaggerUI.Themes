using AspNetCore.Swagger.Themes;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions"/>.
/// </summary>
public static class SwaggerUIOptionsExtensions
{
    private static ThemeSwitcherOptions s_switcherOptions;

    extension(Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions options)
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
            options.EnablePinnableTopbar();
            options.ShowBackToTopButton();
            options.EnableStickyOperations();
            options.EnableExpandOrCollapseAllOperations();
            options.EnableThemeSwitcher(switcherOptions);
        }

        /// <summary>
        /// Enables the pinnable topbar feature.
        /// </summary>
        public void EnablePinnableTopbar() =>
            options.ConfigObject.AdditionalItems.EnablePinnableTopbar();

        /// <summary>
        /// Shows a button to scroll back to the top of the page.
        /// </summary>
        public void ShowBackToTopButton() =>
            options.ConfigObject.AdditionalItems.EnableBackToTop();

        /// <summary>
        /// Enables sticky operations.
        /// </summary>
        public void EnableStickyOperations() =>
            options.ConfigObject.AdditionalItems.EnableStickyOperations();

        /// <summary>
        /// Enables the expand or collapse functionality for all operations inside a tag.
        /// </summary>
        public void EnableExpandOrCollapseAllOperations() =>
            options.ConfigObject.AdditionalItems.EnableExpandOrCollapseAllOperations();

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
            options.ConfigObject.AdditionalItems.EnableThemeSwitcher();

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