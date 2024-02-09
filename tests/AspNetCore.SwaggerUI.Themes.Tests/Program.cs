using AspNetCore.SwaggerUI.Themes;
using AspNetCore.SwaggerUI.Themes.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using static AspNetCore.SwaggerUI.Themes.StyleProvider;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Staging
});
var app = builder.Build();

// Add style endpoint
RegisterTestStyleEndpoint();

await app.RunAsync();

void RegisterTestStyleEndpoint()
{
    foreach (var item in new StyleTestData())
    {
        var style = (Style)item[0];
        var fullPath = StylePath + style.FileName;
        AddGetEndpoint(app, fullPath, GetResourceText(style.FileName));
    }
}

public partial class Program
{
    protected Program()
    { }
}