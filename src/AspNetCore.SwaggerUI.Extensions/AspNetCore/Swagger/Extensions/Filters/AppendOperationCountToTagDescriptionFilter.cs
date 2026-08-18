#if NET10_0_OR_GREATER
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Models;
#endif
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Swagger.Extensions.Filters;

/// <summary>
/// Document filter that appends operation count to tag descriptions in Swagger UI.
/// </summary>
/// <remarks>
/// This filter modifies the OpenAPI document at generation time to include
/// the number of operations (endpoints) in each tag's description.
/// </remarks>
public sealed class AppendOperationCountToTagDescriptionFilter : IDocumentFilter
{
    private readonly string _messageTemplate;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppendOperationCountToTagDescriptionFilter"/> class.
    /// </summary>
    /// <param name="messageTemplate">
    /// The message template to use. Must contain {0} placeholder for the count.
    /// Default: " (operations: {0})"
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="messageTemplate"/> does not contain {0} placeholder.
    /// </exception>
    public AppendOperationCountToTagDescriptionFilter(string messageTemplate = " (operations: {0})")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(messageTemplate);

        if (!messageTemplate.Contains("{0}"))
        {
            throw new ArgumentException("The message template must contain '{0}' placeholder.", nameof(messageTemplate));
        }

        _messageTemplate = messageTemplate;
    }

    /// <summary>
    /// Applies the filter to the OpenAPI document.
    /// </summary>
    /// <param name="swaggerDoc">The OpenAPI document to modify.</param>
    /// <param name="context">The document filter context.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Count operations per tag
        var tagOperationCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in swaggerDoc.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                if (operation?.Tags is null || operation.Tags.Count == 0)
                    continue;

                foreach (var tag in operation.Tags.Select(t => t.Name))
                {
                    if (tagOperationCount.TryGetValue(tag, out int value))
                        tagOperationCount[tag] = ++value;
                    else
                        tagOperationCount[tag] = 1;
                }
            }
        }

        // If no counts found, nothing to do
        if (tagOperationCount.Count == 0)
            return;

        // Ensure Tags collection exists
#if NET10_0_OR_GREATER
        swaggerDoc.Tags ??= new HashSet<OpenApiTag>();
#else
        swaggerDoc.Tags ??= [];
#endif

        // Create tags if they don't exist
        foreach (var tagName in tagOperationCount.Keys)
        {
            if (!swaggerDoc.Tags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
            {
                swaggerDoc.Tags.Add(new OpenApiTag { Name = tagName });
            }
        }

        // Append count to tag descriptions
        foreach (var tag in swaggerDoc.Tags)
        {
            if (tagOperationCount.TryGetValue(tag.Name, out var count))
            {
                tag.Description ??= string.Empty;
                tag.Description += string.Format(_messageTemplate, count);
            }
        }
    }
}