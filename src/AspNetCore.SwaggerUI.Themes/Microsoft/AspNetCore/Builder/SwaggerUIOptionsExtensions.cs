using AspNetCore.Swagger.Themes;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for <see cref="SwaggerUIOptions"/>.
/// </summary>
public static class SwaggerUIOptionsExtensions
{
    /// <summary>
    /// Enables all the advanced options:
    /// <list type="bullet">
    /// <item>
    /// Pinnable topbar:
    /// <see cref="EnablePinnableTopbar(SwaggerUIOptions)"/>
    /// </item>
    /// <item>
    /// Back to top button
    /// <see cref="ShowBackToTopButton(SwaggerUIOptions)"/>
    /// </item>
    /// <item>
    /// Sticky operations
    /// <see cref="EnableStickyOperations(SwaggerUIOptions)"/>
    /// </item>
    /// <item>
    /// Expand or collapse all operations
    /// <see cref="EnableExpandOrCollapseAllOperations(SwaggerUIOptions)"/>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="options">The SwaggerUI options.</param>
    public static void EnableAllAdvancedOptions(this SwaggerUIOptions options)
    {
        options.EnablePinnableTopbar();
        options.ShowBackToTopButton();
        options.EnableStickyOperations();
        options.EnableExpandOrCollapseAllOperations();
    }

    /// <summary>
    /// Enables the pinnable topbar feature.
    /// </summary>
    /// <param name="options">The SwaggerUI options.</param>
    public static void EnablePinnableTopbar(this SwaggerUIOptions options)
    {
        options.ConfigObject.AdditionalItems.EnablePinnableTopbar();
    }

    /// <summary>
    /// Shows a button to scroll back to the top of the page.
    /// </summary>
    /// <param name="options">The SwaggerUI options.</param>
    public static void ShowBackToTopButton(this SwaggerUIOptions options)
    {
        options.ConfigObject.AdditionalItems.EnableBackToTop();
    }

    /// <summary>
    /// Enables sticky operations.
    /// </summary>
    /// <param name="options">The SwaggerUI options.</param>
    public static void EnableStickyOperations(this SwaggerUIOptions options)
    {
        options.ConfigObject.AdditionalItems.EnableStickyOperations();
    }

    /// <summary>
    /// Enables the expand or collapse functionality for all operations inside a tag.
    /// </summary>
    /// <param name="options">The SwaggerUI options.</param>
    public static void EnableExpandOrCollapseAllOperations(this SwaggerUIOptions options)
    {
        options.ConfigObject.AdditionalItems.EnableExpandOrCollapseAllOperations();
    }
}