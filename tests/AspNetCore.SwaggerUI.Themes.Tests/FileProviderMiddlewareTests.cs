using AspNetCore.Swagger.Themes.Tests.Utilities;
using Shouldly;
using System.Net;
using static AspNetCore.Swagger.Themes.FileProvider;

namespace AspNetCore.Swagger.Themes.Tests;

/// <summary>
/// Tests for FileProvider with non-WebApplication scenarios (middleware-based).
/// These tests are isolated to avoid conflicts with WebApplication endpoint registrations.
/// </summary>
public class FileProviderMiddlewareTests : IClassFixture<ThemeProviderWebApplicationFactory<Program>>
{
    private readonly ThemeProviderWebApplicationFactory<Program> _themeProviderWebApplicationFactory;

    public FileProviderMiddlewareTests(ThemeProviderWebApplicationFactory<Program> themeProviderWebApplicationFactory)
    {
        _themeProviderWebApplicationFactory = themeProviderWebApplicationFactory;
        _themeProviderWebApplicationFactory.CreateClient();
    }

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

    [Fact]
    public async Task AddGetEndpoint_ShouldResolveEachPathIndependently_WhenMultiplePathsRegisteredOnSameApp()
    {
        // Arrange
        var mockAppBuilder = new MockApplicationBuilder();
        var prefix = $"/test-middleware-multi-{Guid.NewGuid():N}";
        var registrations = Enumerable.Range(0, 6)
            .Select(i => (
                Path: $"{prefix}/{i}.css",
                Content: $"body {{ /* asset {i} */ }}",
                ContentType: i % 2 == 0 ? MimeTypes.Text.Css : MimeTypes.Text.Javascript))
            .ToList();

        foreach (var (path, content, contentType) in registrations)
        {
            AddGetEndpoint(mockAppBuilder, path, content, contentType);
        }

        var app = mockAppBuilder.Build();

        // Act & Assert - each registered path resolves to its own content and content-type
        foreach (var (path, content, contentType) in registrations)
        {
            var context = MockApplicationBuilder.CreateHttpContext(path);
            await app.Invoke(context);
            await context.Response.Body.FlushAsync();

            context.Response.StatusCode.ShouldBe(200);
            context.Response.ContentType.ShouldBe(contentType);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            (await reader.ReadToEndAsync()).ShouldBe(content);
        }

        // An unregistered path still falls through to the terminal 404
        var missingContext = MockApplicationBuilder.CreateHttpContext($"{prefix}/unregistered.css");
        await app.Invoke(missingContext);

        missingContext.Response.StatusCode.ShouldBe(404);
    }

    [Fact]
    public async Task AddGetEndpoint_ShouldResolveEachPathIndependently_WhenWebApplication()
    {
        // Arrange - the shared test WebApplication (see Program.cs) registers one path per
        // theme via AddGetEndpoint at startup, all served through the same dispatch middleware.
        var registeredThemes = new ThemeTestData();

        // Act & Assert - each theme's style path resolves through the same WebApplication instance
        foreach (var theme in registeredThemes)
        {
            var fullPath = StylesPath + theme.FileName;
            var expectedContent = GetResourceText(theme.FileName, theme.GetType());

            var response = await _themeProviderWebApplicationFactory.Client.GetAsync(fullPath);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.ShouldBe(MimeTypes.Text.Css);
            (await response.Content.ReadAsStringAsync()).ShouldBeEquivalentTo(expectedContent);
        }

        // An unregistered path still falls through to the app's default 404 handling
        var missingResponse = await _themeProviderWebApplicationFactory.Client.GetAsync(
            $"{StylesPath}unregistered-{Guid.NewGuid():N}.css");

        missingResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddGetEndpoint_WhenCalledConcurrently_RegistersEveryEndpointExactlyOnce()
    {
        // Arrange
        const int EndpointCount = 50;
        var prefix = $"/test-middleware-concurrent-{Guid.NewGuid():N}";
        var registrations = Enumerable.Range(0, EndpointCount)
            .Select(i => (Path: $"{prefix}/{i}.css", AppBuilder: new MockApplicationBuilder()))
            .ToList();

        // Act - each registration uses its own builder so the middleware pipeline itself
        // isn't mutated concurrently; only the shared static endpoint registry is stressed.
        await Should.NotThrowAsync(() => Task.WhenAll(
            registrations.Select(r => Task.Run(() =>
                AddGetEndpoint(r.AppBuilder, r.Path, $"body {{ /* {r.Path} */ }}")))));

        // Assert
        foreach (var (path, appBuilder) in registrations)
        {
            var app = appBuilder.Build();
            var context = MockApplicationBuilder.CreateHttpContext(path);
            await app.Invoke(context);

            context.Response.StatusCode.ShouldBe(200);
        }
    }
}