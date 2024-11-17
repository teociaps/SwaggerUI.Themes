namespace AspNetCore.Swagger.Themes.Tests;

using Shouldly;
using System.Collections.Generic;
using Xunit;

public class AdvancedOptionsTests
{
    [Fact]
    public void EnablePinnableTopbar_AddsCorrectOption()
    {
        // Arrange
        var options = new Dictionary<string, object>();

        // Act
        options.EnablePinnableTopbar();

        // Assert
        options.ShouldContainKey(AdvancedOptions.PinnableTopbar);
        options[AdvancedOptions.PinnableTopbar].ShouldBe(true);
    }

    [Fact]
    public void EnableBackToTop_AddsCorrectOption()
    {
        // Arrange
        var options = new Dictionary<string, object>();

        // Act
        options.EnableBackToTop();

        // Assert
        options.ShouldContainKey(AdvancedOptions.BackToTop);
        options[AdvancedOptions.BackToTop].ShouldBe(true);
    }

    [Fact]
    public void EnableStickyOperations_AddsCorrectOption()
    {
        // Arrange
        var options = new Dictionary<string, object>();

        // Act
        options.EnableStickyOperations();

        // Assert
        options.ShouldContainKey(AdvancedOptions.StickyOperations);
        options[AdvancedOptions.StickyOperations].ShouldBe(true);
    }

    [Fact]
    public void EnableExpandOrCollapseAllOperations_AddsCorrectOption()
    {
        // Arrange
        var options = new Dictionary<string, object>();

        // Act
        options.EnableExpandOrCollapseAllOperations();

        // Assert
        options.ShouldContainKey(AdvancedOptions.ExpandOrCollapseAllOperations);
        options[AdvancedOptions.ExpandOrCollapseAllOperations].ShouldBe(true);
    }

    [Fact]
    public void AnyJsFeatureEnabled_ReturnsTrue_WhenFeatureIsEnabled()
    {
        // Arrange
        var options = new Dictionary<string, object>
        {
            { AdvancedOptions.PinnableTopbar, true }
        };

        // Act
        var result = AdvancedOptions.AnyJsFeatureEnabled(options);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void AnyJsFeatureEnabled_ReturnsFalse_WhenNoFeatureIsEnabled()
    {
        // Arrange
        var options = new Dictionary<string, object>();

        // Act
        var result = AdvancedOptions.AnyJsFeatureEnabled(options);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Apply_AppliesCssFeaturesCorrectly()
    {
        // Arrange
        const string CssContent = $$""""""body { margin: 0; } {{AdvancedOptions.StickyOperationsCssPH}} { display: none; }"""""";
        var options = new Dictionary<string, object>
        {
            { AdvancedOptions.StickyOperations, true }
        };

        // Act
        var result = AdvancedOptions.Apply(CssContent, options, MimeTypes.Text.Css);

        // Assert
        result.ShouldNotContain(AdvancedOptions.StickyOperationsCssPH);
    }

    [Fact]
    public void Apply_AppliesJsFeaturesCorrectly()
    {
        // Arrange
        const string JsContent = $"""
            const pinnableTopbapFeature = {AdvancedOptions.PinnableTopbarJsPH};
            const backToTopFeature = {AdvancedOptions.BackToTopJsPH};
            const expandOrCollapseOpsFeature = {AdvancedOptions.ExpandOrCollapseAllOperationsJsPH};
        """;
        var options = new Dictionary<string, object>
        {
            { AdvancedOptions.PinnableTopbar, true },
            { AdvancedOptions.BackToTop, true },
            { AdvancedOptions.ExpandOrCollapseAllOperations, true }
        };

        // Act
        var result = AdvancedOptions.Apply(JsContent, options, MimeTypes.Text.Javascript);

        const string ExpectedJsConent = """
            const pinnableTopbapFeature = true;
            const backToTopFeature = true;
            const expandOrCollapseOpsFeature = true;
        """;

        // Assert
        result.ShouldBe(ExpectedJsConent);
        result.ShouldNotContain(AdvancedOptions.PinnableTopbarJsPH);
    }

    [Fact]
    public void Apply_DoesNotChangeContent_WhenUnsupportedMimeTypeIsUsed()
    {
        // Arrange
        const string Content = "unsupported content";
        var options = new Dictionary<string, object>();

        // Act
        var result = AdvancedOptions.Apply(Content, options, "unsupported/mime-type");

        // Assert
        result.ShouldBe(Content);
    }
}
