using AspNetCore.Swagger.Extensions.Processors;
using NSwag.Generation.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring OpenAPI document generation with operation count feature.
/// </summary>
public static class AspNetCoreOpenApiDocumentGeneratorSettingsExtensions
{
    /// <summary>
    /// Appends operation count to tag descriptions in Swagger UI.
    /// </summary>
    /// <param name="settings">The OpenAPI document generator settings.</param>
    /// <param name="messageTemplate">
    /// Optional message template. Must contain {0} placeholder for the count.
    /// Default: " (operations: {0})"
    /// </param>
    /// <example>
    /// <code>
    /// services.AddOpenApiDocument(c =>
    /// {
    ///     c.AppendOperationCountToTags();
    ///     // Or with custom template:
    ///     c.AppendOperationCountToTags(" [{0} endpoints]");
    /// });
    /// </code>
    /// </example>
    public static void AppendOperationCountToTags(
        this AspNetCoreOpenApiDocumentGeneratorSettings settings,
        string messageTemplate = " (operations: {0})")
    {
        settings.DocumentProcessors.Add(
            new AppendOperationCountToTagDescriptionProcessor(messageTemplate));
    }
}
