using AspNetCore.Swagger.Themes;
using AspNetCore.Swagger.Themes.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using static AspNetCore.Swagger.Themes.FileProvider;

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
    foreach (var style in new StyleTestData())
    {
        var fullPath = StylesPath + style.FileName;
        AddGetEndpoint(app, fullPath, GetResourceText(style.FileName, style.GetType()));
        if (style.LoadAdditionalJs)
            AddGetEndpoint(app, ScriptsPath + "modern.js", GetResourceText("modern.js"), MimeTypes.Text.Javascript);
    }
}

public partial class Program
{
    protected Program()
    { }
}