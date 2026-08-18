using Microsoft.AspNetCore.Builder;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;

namespace AspNetCore.Swagger.Themes.Tests;

public class SwaggerUIOptionsExtensionsTests
{
    [Fact]
    public void EnableFilter_Pinnable_AddsPinnableFilterBarOption()
    {
        // Arrange
        var options = new SwaggerUIOptions();

        // Act
        options.EnableFilter(pinnable: true);

        // Assert
        options.ConfigObject.AdditionalItems.ShouldContainKey(AdvancedOptions.PinnableFilterBar);
    }

    [Fact]
    public void EnableFilter_NotPinnable_DoesNotAddPinnableFilterBarOption()
    {
        // Arrange
        var options = new SwaggerUIOptions();

        // Act
        options.EnableFilter(pinnable: false);

        // Assert
        options.ConfigObject.AdditionalItems.ShouldNotContainKey(AdvancedOptions.PinnableFilterBar);
    }
}
