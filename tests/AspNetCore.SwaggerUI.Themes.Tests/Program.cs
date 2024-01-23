using AspNetCore.SwaggerUI.Themes;
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
    var style = Style.Dark;
    var fullPath = StylePath + style.FileName;
    AddGetEndpoint(app, fullPath, GetResourceText(style.FileName), style.FormatText);
}

public partial class Program
{
    protected Program()
    { }
}