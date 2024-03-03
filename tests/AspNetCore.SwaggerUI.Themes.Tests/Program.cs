using AspNetCore.SwaggerUI.Themes;
using AspNetCore.SwaggerUI.Themes.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using static AspNetCore.SwaggerUI.Themes.FileProvider;

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
        var style = (BaseStyle)item[0];
        var fullPath = StylesPath + style.FileName;
        AddGetEndpoint(app, fullPath, GetResourceText(style.FileName));
        if (style.IsModern)
            AddGetEndpoint(app, ScriptsPath + "modern.js", GetResourceText("modern.js"), MimeTypes.Text.Javascript);
    }
}

public partial class Program
{
    protected Program()
    { }
}