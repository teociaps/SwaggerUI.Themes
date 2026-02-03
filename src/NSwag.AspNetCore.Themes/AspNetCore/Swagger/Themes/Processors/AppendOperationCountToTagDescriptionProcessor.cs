using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace AspNetCore.Swagger.Themes.Processors;

/// <summary>
/// Document processor that appends operation count to tag descriptions in Swagger UI.
/// </summary>
/// <remarks>
/// This processor modifies the OpenAPI document at generation time to include
/// the number of operations (endpoints) in each tag's description.
/// Inspired by: https://github.com/unchase/Unchase.Swashbuckle.AspNetCore.Extensions
/// </remarks>
public sealed class AppendOperationCountToTagDescriptionProcessor : IDocumentProcessor
{
    private readonly string _messageTemplate;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppendOperationCountToTagDescriptionProcessor"/> class.
    /// </summary>
    /// <param name="messageTemplate">
    /// The message template to use. Must contain {0} placeholder for the count.
    /// Default: " (operations: {0})"
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="messageTemplate"/> does not contain {0} placeholder.
    /// </exception>
    public AppendOperationCountToTagDescriptionProcessor(string messageTemplate = " (operations: {0})")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(messageTemplate);

        if (!messageTemplate.Contains("{0}"))
        {
            throw new ArgumentException("The message template must contain '{0}' placeholder.", nameof(messageTemplate));
        }

        _messageTemplate = messageTemplate;
    }

    /// <summary>
    /// Processes the OpenAPI document.
    /// </summary>
    /// <param name="context">The document processor context.</param>
    public void Process(DocumentProcessorContext context)
    {
        var document = context.Document;

        // Count operations per tag
        var tagOperationCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in document.Paths.Values)
        {
            foreach (var operation in path.Values)
            {
                if (operation?.Tags is null || operation.Tags.Count == 0)
                    continue;

                foreach (var tag in operation.Tags)
                {
                    if (tagOperationCount.ContainsKey(tag))
                        tagOperationCount[tag]++;
                    else
                        tagOperationCount[tag] = 1;
                }
            }
        }

        // If no counts found, nothing to do
        if (tagOperationCount.Count == 0)
            return;

        // Ensure Tags collection exists
        document.Tags ??= new List<NSwag.OpenApiTag>();

        // Create tags if they don't exist
        foreach (var tagName in tagOperationCount.Keys)
        {
            if (!document.Tags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
            {
                document.Tags.Add(new NSwag.OpenApiTag { Name = tagName });
            }
        }

        // Append count to tag descriptions
        foreach (var tag in document.Tags)
        {
            if (tagOperationCount.TryGetValue(tag.Name, out var count))
            {
                tag.Description ??= string.Empty;
                tag.Description += string.Format(_messageTemplate, count);
            }
        }
    }
}
