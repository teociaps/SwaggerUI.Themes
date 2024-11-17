using AspNetCore.Swagger.Themes;
using NSwag.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="SwaggerUiSettings"/>.
/// </summary>
public static class SwaggerUiSettingsExtensions
{
    /// <summary>
    /// Enables all the advanced options:
    /// <list type="bullet">
    /// <item>
    /// Pinnable topbar:
    /// <see cref="EnablePinnableTopbar(SwaggerUiSettings)"/>
    /// </item>
    /// <item>
    /// Back to top button
    /// <see cref="ShowBackToTopButton(SwaggerUiSettings)"/>
    /// </item>
    /// <item>
    /// Sticky operations
    /// <see cref="EnableStickyOperations(SwaggerUiSettings)"/>
    /// </item>
    /// <item>
    /// Expand or collapse all operations
    /// <see cref="EnableExpandOrCollapseAllOperations(SwaggerUiSettings)"/>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="settings">The SwaggerUi options.</param>
    public static void EnableAllAdvancedOptions(this SwaggerUiSettings settings)
    {
        settings.EnablePinnableTopbar();
        settings.ShowBackToTopButton();
        settings.EnableStickyOperations();
        settings.EnableExpandOrCollapseAllOperations();
    }

    /// <summary>
    /// Enables the pinnable topbar feature.
    /// </summary>
    /// <param name="settings">The SwaggerUi settings.</param>
    public static void EnablePinnableTopbar(this SwaggerUiSettings settings)
    {
        settings.AdditionalSettings.EnablePinnableTopbar();
    }

    /// <summary>
    /// Shows a button to scroll back to the top of the page.
    /// </summary>
    /// <param name="settings">The SwaggerUi settings.</param>
    public static void ShowBackToTopButton(this SwaggerUiSettings settings)
    {
        settings.AdditionalSettings.EnableBackToTop();
    }

    /// <summary>
    /// Enables sticky operations.
    /// </summary>
    /// <param name="settings">The SwaggerUi settings.</param>
    public static void EnableStickyOperations(this SwaggerUiSettings settings)
    {
        settings.AdditionalSettings.EnableStickyOperations();
    }

    /// <summary>
    /// Enables the expand or collapse functionality for all operations inside a tag.
    /// </summary>
    /// <param name="settings">The SwaggerUi settings.</param>
    public static void EnableExpandOrCollapseAllOperations(this SwaggerUiSettings settings)
    {
        settings.AdditionalSettings.EnableExpandOrCollapseAllOperations();
    }
}