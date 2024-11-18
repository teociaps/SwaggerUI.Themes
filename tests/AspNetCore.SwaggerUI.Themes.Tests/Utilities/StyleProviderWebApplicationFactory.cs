using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace AspNetCore.Swagger.Themes.Tests.Utilities;

/// <summary>
/// Factory for bootstrapping an application in memory for functional end to end tests.
/// </summary>
/// <typeparam name="TProgram">
/// A type in the entry point assembly of the application. Typically the Program classes can be used.
/// </typeparam>
public class StyleProviderWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    /// <summary>
    /// HTTP client for executing tests.
    /// </summary>
    public HttpClient Client { get; set; }

    /// <summary>
    /// Creates an instance of <see cref="HttpClient "/> to be used during tests.
    /// </summary>
    public new void CreateClient()
    {
        Client = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    /// <inheritdoc/>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }
}