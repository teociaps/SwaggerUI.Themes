using AspNetCore.Swagger.Themes.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring Swagger generation with operation count feature.
/// </summary>
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Appends operation count to tag descriptions in Swagger UI.
    /// </summary>
    /// <param name="options">The Swagger generation options.</param>
    /// <param name="messageTemplate">
    /// Optional message template. Must contain {0} placeholder for the count.
    /// Default: " (operations: {0})"
    /// </param>
    /// <example>
    /// <code>
    /// services.AddSwaggerGen(c =>
    /// {
    ///     c.AppendOperationCountToTags();
    ///     // Or with custom template:
    ///     c.AppendOperationCountToTags(" [{0} endpoints]");
    /// });
    /// </code>
    /// </example>
    public static void AppendOperationCountToTags(
        this SwaggerGenOptions options,
        string messageTemplate = " (operations: {0})")
    {
        options.DocumentFilter<AppendOperationCountToTagDescriptionFilter>(messageTemplate);
    }
}
