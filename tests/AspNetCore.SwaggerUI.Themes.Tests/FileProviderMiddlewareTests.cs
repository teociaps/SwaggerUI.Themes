using AspNetCore.Swagger.Themes.Tests.Utilities;
using Shouldly;
using static AspNetCore.Swagger.Themes.FileProvider;

namespace AspNetCore.Swagger.Themes.Tests;

/// <summary>
/// Tests for FileProvider with non-WebApplication scenarios (middleware-based).
/// These tests are isolated to avoid conflicts with WebApplication endpoint registrations.
/// </summary>
public class FileProviderMiddlewareTests
{
    private readonly Dictionary<string, object> _advancedOptions = new()
    {
        { AdvancedOptions.PinnableTopbar, true },
        { AdvancedOptions.StickyOperations, true },
        { AdvancedOptions.BackToTop, true },
        { AdvancedOptions.ExpandOrCollapseAllOperations, true }
    };

    [Theory]
    [ClassData(typeof(ThemeTestData))]
    public async Task AddGetEndpoint_ShouldReturnCssContent_WhenNotWebApplication(BaseTheme theme)
    {
        // Arrange
        var mockAppBuilder = new MockApplicationBuilder();
        var path = $"/test-middleware{StylesPath}{theme.FileName}"; // Use unique path to avoid conflicts
        var content = GetResourceText(theme.FileName, theme.GetType());

        // Act
        AddGetEndpoint(mockAppBuilder, path, content);
        var app = mockAppBuilder.Build();

        // Simulate a request
        var context = MockApplicationBuilder.CreateHttpContext(path);
        await app.Invoke(context);
        await context.Response.Body.FlushAsync();

        // Assert
        context.Response.StatusCode.ShouldBe(200);
        context.Response.ContentType.ShouldBe(MimeTypes.Text.Css);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        responseBody.ShouldBe(content);
    }

    [Fact]
    public async Task AddGetEndpoint_ShouldReturn404_WhenPathDoesNotMatch()
    {
        // Arrange
        var mockAppBuilder = new MockApplicationBuilder();
        const string registeredPath = "/test-middleware/styles/dark.min.css";
        const string requestPath = "/test-middleware/styles/light.min.css";
        const string content = "body { background: #000; }";

        // Act
        AddGetEndpoint(mockAppBuilder, registeredPath, content);
        var app = mockAppBuilder.Build();

        var context = MockApplicationBuilder.CreateHttpContext(requestPath);
        await app.Invoke(context);

        // Assert
        context.Response.StatusCode.ShouldBe(404); // Middleware didn't match, falls through to 404
    }

    [Fact]
    public async Task AddGetEndpoint_ShouldSetCacheHeaders()
    {
        // Arrange
        var mockAppBuilder = new MockApplicationBuilder();
        const string path = "/test-middleware/cached-style.css";
        const string content = "body { }";

        // Act
        AddGetEndpoint(mockAppBuilder, path, content);
        var app = mockAppBuilder.Build();

        var context = MockApplicationBuilder.CreateHttpContext(path);
        await app.Invoke(context);

        // Assert
        context.Response.Headers.ShouldContainKey("Cache-Control");
        context.Response.Headers["Cache-Control"].ToString().ShouldBe("max-age=3600");

        context.Response.Headers.ShouldContainKey("Expires");
        context.Response.Headers["Expires"].ToString().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task AddGetEndpoint_ShouldHandleCustomContentType()
    {
        // Arrange
        var mockAppBuilder = new MockApplicationBuilder();
        const string path = "/test-middleware/script.js";
        const string content = "console.log('test');";
        const string contentType = MimeTypes.Text.Javascript;

        // Act
        AddGetEndpoint(mockAppBuilder, path, content, contentType);
        var app = mockAppBuilder.Build();

        var context = MockApplicationBuilder.CreateHttpContext(path);
        await app.Invoke(context);

        // Assert
        context.Response.StatusCode.ShouldBe(200);
        context.Response.ContentType.ShouldBe(contentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        responseBody.ShouldBe(content);
    }
}