using AspNetCore.Swagger.Extensions.Filters;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace AspNetCore.Swagger.Extensions.Tests;

public class SwaggerGenOptionsExtensionsTests
{
    [Fact]
    public void AppendOperationCountToTags_AddsFilter()
    {
        // Arrange
        var options = new SwaggerGenOptions();

        // Act
        options.AppendOperationCountToTags();

        // Assert
        options.DocumentFilterDescriptors.ShouldNotBeEmpty();
        options.DocumentFilterDescriptors.ShouldContain(d =>
            d.Type == typeof(AppendOperationCountToTagDescriptionFilter));
    }

    [Fact]
    public void AppendOperationCountToTags_WithCustomTemplate_AddsFilterWithTemplate()
    {
        // Arrange
        var options = new SwaggerGenOptions();
        var customTemplate = " [{0} endpoints]";

        // Act
        options.AppendOperationCountToTags(customTemplate);

        // Assert
        options.DocumentFilterDescriptors.ShouldNotBeEmpty();
        var descriptor = options.DocumentFilterDescriptors
            .FirstOrDefault(d => d.Type == typeof(AppendOperationCountToTagDescriptionFilter));
        descriptor.ShouldNotBeNull();
        descriptor.Arguments.ShouldContain(customTemplate);
    }
}