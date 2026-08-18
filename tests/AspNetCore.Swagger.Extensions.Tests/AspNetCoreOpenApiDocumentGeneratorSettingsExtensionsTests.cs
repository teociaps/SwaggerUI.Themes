using AspNetCore.Swagger.Extensions.Processors;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.AspNetCore;
using Shouldly;
using Xunit;

namespace AspNetCore.Swagger.Extensions.Tests;

public class AspNetCoreOpenApiDocumentGeneratorSettingsExtensionsTests
{
    [Fact]
    public void AppendOperationCountToTags_AddsProcessor()
    {
        // Arrange
        var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();

        // Act
        settings.AppendOperationCountToTags();

        // Assert
        settings.DocumentProcessors.ShouldNotBeEmpty();
        settings.DocumentProcessors.ShouldContain(p =>
            p.GetType() == typeof(AppendOperationCountToTagDescriptionProcessor));
    }

    [Fact]
    public void AppendOperationCountToTags_WithCustomTemplate_AddsProcessorWithTemplate()
    {
        // Arrange
        var settings = new AspNetCoreOpenApiDocumentGeneratorSettings();
        var customTemplate = " [{0} endpoints]";

        // Act
        settings.AppendOperationCountToTags(customTemplate);

        // Assert
        settings.DocumentProcessors.ShouldNotBeEmpty();
        var processor = settings.DocumentProcessors
            .FirstOrDefault(p => p.GetType() == typeof(AppendOperationCountToTagDescriptionProcessor));
        processor.ShouldNotBeNull();
    }
}