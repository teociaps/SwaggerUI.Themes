using Microsoft.AspNetCore.Builder;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;

namespace AspNetCore.Swagger.Themes.Tests;

public class SwaggerUIOptionsExtensionsTests
{
    [Fact]
    public void EnableFilter_Pinned_AddsPinnableFilterBarOption()
    {
        // Arrange
        var options = new SwaggerUIOptions();

        // Act
        options.EnableFilter(pinned: true);

        // Assert
        options.ConfigObject.AdditionalItems.ShouldContainKey(AdvancedOptions.PinnableFilterBar);
    }

    [Fact]
    public void EnableFilter_NotPinned_DoesNotAddPinnableFilterBarOption()
    {
        // Arrange
        var options = new SwaggerUIOptions();

        // Act
        options.EnableFilter(pinned: false);

        // Assert
        options.ConfigObject.AdditionalItems.ShouldNotContainKey(AdvancedOptions.PinnableFilterBar);
    }
}
