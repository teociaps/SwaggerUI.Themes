using AspNetCore.Swagger.Extensions.Filters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
using Xunit;

namespace AspNetCore.Swagger.Extensions.Tests;

public class AppendOperationCountToTagDescriptionFilterTests
{
    [Fact]
    public void Constructor_WithValidTemplate_Succeeds()
    {
        // Act
        var filter = new AppendOperationCountToTagDescriptionFilter(" (operations: {0})");

        // Assert
        filter.ShouldNotBeNull();
    }

    [Fact]
    public void Constructor_WithNullTemplate_ThrowsArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            new AppendOperationCountToTagDescriptionFilter(null!));
    }

    [Fact]
    public void Constructor_WithEmptyTemplate_ThrowsArgumentException()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            new AppendOperationCountToTagDescriptionFilter(""));
    }

    [Fact]
    public void Constructor_WithoutPlaceholder_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() =>
            new AppendOperationCountToTagDescriptionFilter(" operations"));
        exception.Message.ShouldContain("{0}");
    }

    [Fact]
    public void Apply_WithEmptyDocument_DoesNothing()
    {
        // Arrange
        var filter = new AppendOperationCountToTagDescriptionFilter();
        var document = new OpenApiDocument
        {
            Paths = []
        };
        var context = CreateContext();

        // Act
        filter.Apply(document, context);

        // Assert
        document.Tags.ShouldBeEmpty();
    }

    [Fact]
    public void Apply_WithOperations_AppendsCount()
    {
        // Arrange
        var filter = new AppendOperationCountToTagDescriptionFilter();
        var document = CreateDocumentWithTags();
        var context = CreateContext();

        // Act
        filter.Apply(document, context);

        // Assert
        document.Tags.ShouldNotBeEmpty();
        var userTag = document.Tags.FirstOrDefault(t => t.Name == "Users");
        userTag.ShouldNotBeNull();
        userTag.Description.ShouldContain("(operations: 2)");
    }

    [Fact]
    public void Apply_WithCustomTemplate_UsesTemplate()
    {
        // Arrange
        var filter = new AppendOperationCountToTagDescriptionFilter(" [{0} endpoints]");
        var document = CreateDocumentWithTags();
        var context = CreateContext();

        // Act
        filter.Apply(document, context);

        // Assert
        var userTag = document.Tags.FirstOrDefault(t => t.Name == "Users");
        userTag.ShouldNotBeNull();
        userTag.Description.ShouldContain("[2 endpoints]");
    }

    [Fact]
    public void Apply_WithExistingDescription_AppendsCount()
    {
        // Arrange
        var filter = new AppendOperationCountToTagDescriptionFilter();
        var document = CreateDocumentWithTags();
        document.Tags = new List<OpenApiTag>
        {
            new() { Name = "Users", Description = "User management" }
        };
        var context = CreateContext();

        // Act
        filter.Apply(document, context);

        // Assert
        var userTag = document.Tags.FirstOrDefault(t => t.Name == "Users");
        userTag.ShouldNotBeNull();
        userTag.Description.ShouldBe("User management (operations: 2)");
    }

    [Fact]
    public void Apply_WithMultipleTags_CountsCorrectly()
    {
        // Arrange
        var filter = new AppendOperationCountToTagDescriptionFilter();
        var document = new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/users"] = new OpenApiPathItem
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new() { Tags = new List<OpenApiTag> { new() { Name = "Users" } } },
                        [OperationType.Post] = new() { Tags = new List<OpenApiTag> { new() { Name = "Users" } } }
                    }
                },
                ["/products"] = new OpenApiPathItem
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new() { Tags = new List<OpenApiTag> { new() { Name = "Products" } } }
                    }
                }
            }
        };
        var context = CreateContext();

        // Act
        filter.Apply(document, context);

        // Assert
        document.Tags.Count.ShouldBe(2);
        document.Tags.First(t => t.Name == "Users").Description.ShouldContain("(operations: 2)");
        document.Tags.First(t => t.Name == "Products").Description.ShouldContain("(operations: 1)");
    }

    private static DocumentFilterContext CreateContext()
    {
        return new DocumentFilterContext(
            new List<ApiDescription>(),
            new SchemaGenerator(new SchemaGeneratorOptions(), new JsonSerializerDataContractResolver(new JsonSerializerOptions())),
            new SchemaRepository());
    }

    private static OpenApiDocument CreateDocumentWithTags()
    {
        return new OpenApiDocument
        {
            Paths = new OpenApiPaths
            {
                ["/users"] = new OpenApiPathItem
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new()
                        {
                            Tags = new List<OpenApiTag> { new() { Name = "Users" } }
                        },
                        [OperationType.Post] = new()
                        {
                            Tags = new List<OpenApiTag> { new() { Name = "Users" } }
                        }
                    }
                }
            }
        };
    }
}