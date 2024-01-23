using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace AspNetCore.SwaggerUI.Themes.Tests;

/// <summary>
/// Factory for bootstrapping an application in memory for functional end to end tests.
/// </summary>
/// <typeparam name="TProgram">
/// A type in the entry point assembly of the application. Typically the Program classes can be used.
/// </typeparam>
public abstract class StyleProviderWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    /// <summary>
    /// HTTP client for executing tests.
    /// </summary>
    protected HttpClient Client { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleProviderWebApplicationFactory{TProgram}"/> class.
    /// </summary>
    protected StyleProviderWebApplicationFactory()
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