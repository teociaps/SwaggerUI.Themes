using AspNetCore.Swagger.Extensions.Processors;
using NSwag;
using NSwag.Generation.Processors.Contexts;
using Shouldly;
using Xunit;

namespace AspNetCore.Swagger.Extensions.Tests;

public class AppendOperationCountToTagDescriptionProcessorTests
{
    [Fact]
    public void Constructor_WithValidTemplate_Succeeds()
    {
        // Act
        var processor = new AppendOperationCountToTagDescriptionProcessor(" (operations: {0})");

        // Assert
        processor.ShouldNotBeNull();
    }

    [Fact]
    public void Constructor_WithNullTemplate_ThrowsArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            new AppendOperationCountToTagDescriptionProcessor(null!));
    }

    [Fact]
    public void Constructor_WithEmptyTemplate_ThrowsArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            new AppendOperationCountToTagDescriptionProcessor(""));
    }

    [Fact]
    public void Constructor_WithoutPlaceholder_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() =>
            new AppendOperationCountToTagDescriptionProcessor(" operations"));
        exception.Message.ShouldContain("{0}");
    }

    [Fact]
    public void Process_WithEmptyDocument_DoesNothing()
    {
        // Arrange
        var processor = new AppendOperationCountToTagDescriptionProcessor();
        var document = new OpenApiDocument
        {
            Tags = []
        };
        var context = CreateContext(document);

        // Act
        processor.Process(context);

        // Assert
        document.Tags.ShouldBeEmpty();
    }

    [Fact]
    public void Process_WithOperations_AppendsCount()
    {
        // Arrange
        var processor = new AppendOperationCountToTagDescriptionProcessor();
        var document = CreateDocumentWithTags();
        var context = CreateContext(document);

        // Act
        processor.Process(context);

        // Assert
        document.Tags.ShouldNotBeEmpty();
        var userTag = document.Tags.FirstOrDefault(t => t.Name == "Users");
        userTag.ShouldNotBeNull();
        userTag.Description.ShouldContain("(operations: 2)");
    }

    [Fact]
    public void Process_WithCustomTemplate_UsesTemplate()
    {
        // Arrange
        var processor = new AppendOperationCountToTagDescriptionProcessor(" [{0} endpoints]");
        var document = CreateDocumentWithTags();
        var context = CreateContext(document);

        // Act
        processor.Process(context);

        // Assert
        var userTag = document.Tags.FirstOrDefault(t => t.Name == "Users");
        userTag.ShouldNotBeNull();
        userTag.Description.ShouldContain("[2 endpoints]");
    }

    [Fact]
    public void Process_WithExistingDescription_AppendsCount()
    {
        // Arrange
        var processor = new AppendOperationCountToTagDescriptionProcessor();
        var document = CreateDocumentWithTags();
        document.Tags = new List<OpenApiTag>
        {
            new() { Name = "Users", Description = "User management" }
        };
        var context = CreateContext(document);

        // Act
        processor.Process(context);

        // Assert
        var userTag = document.Tags.FirstOrDefault(t => t.Name == "Users");
        userTag.ShouldNotBeNull();
        userTag.Description.ShouldBe("User management (operations: 2)");
    }

    [Fact]
    public void Process_WithMultipleTags_CountsCorrectly()
    {
        // Arrange
        var processor = new AppendOperationCountToTagDescriptionProcessor();
        var document = new OpenApiDocument();
        document.Paths.Add("/users", new OpenApiPathItem
        {
            {
                OpenApiOperationMethod.Get,
                new OpenApiOperation { Tags = new List<string> { "Users" } }
            },
            {
                OpenApiOperationMethod.Post,
                new OpenApiOperation { Tags = new List<string> { "Users" } }
            }
        });
        document.Paths.Add("/products", new OpenApiPathItem
        {
            {
                OpenApiOperationMethod.Get,
                new OpenApiOperation { Tags = new List<string> { "Products" } }
            }
        });
        var context = CreateContext(document);

        // Act
        processor.Process(context);

        // Assert
        document.Tags.Count.ShouldBe(2);
        document.Tags.First(t => t.Name == "Users").Description.ShouldContain("(operations: 2)");
        document.Tags.First(t => t.Name == "Products").Description.ShouldContain("(operations: 1)");
    }

    private static DocumentProcessorContext CreateContext(OpenApiDocument document)
    {
        return new DocumentProcessorContext(
            document,
            new List<Type>(),
            new List<Type>(),
            null!,
            null!,
            null!);
    }

    private static OpenApiDocument CreateDocumentWithTags()
    {
        var document = new OpenApiDocument();
        document.Paths.Add("/users", new OpenApiPathItem
        {
            {
                OpenApiOperationMethod.Get,
                new OpenApiOperation { Tags = new List<string> { "Users" } }
            },
            {
                OpenApiOperationMethod.Post,
                new OpenApiOperation { Tags = new List<string> { "Users" } }
            }
        });
        return document;
    }
}