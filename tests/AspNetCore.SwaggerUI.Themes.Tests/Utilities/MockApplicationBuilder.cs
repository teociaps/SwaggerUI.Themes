using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace AspNetCore.Swagger.Themes.Tests.Utilities;

public class MockApplicationBuilder : IApplicationBuilder
{
    private readonly IList<Func<RequestDelegate, RequestDelegate>> _components = [];

    public IServiceProvider ApplicationServices { get; set; }
    public IFeatureCollection ServerFeatures { get; set; } = new FeatureCollection();
    public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

    public RequestDelegate Build()
    {
        RequestDelegate app = context =>
        {
            context.Response.StatusCode = 404;
            return Task.CompletedTask;
        };

        foreach (var component in _components.Reverse())
        {
            app = component(app);
        }

        return app;
    }

    public IApplicationBuilder New() => new MockApplicationBuilder();

    public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        _components.Add(middleware);
        return this;
    }

    public static HttpContext CreateHttpContext(string path)
    {
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Response.Body = new MemoryStream(); // To capture the response body
        return context;
    }
}