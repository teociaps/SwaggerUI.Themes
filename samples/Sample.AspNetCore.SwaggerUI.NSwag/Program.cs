using AspNetCore.Swagger.Themes;
using Sample.AspNetCore.SwaggerUI.NSwag;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddOpenApiDocument(OpenApiDocGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    //app.UseSwaggerUi(CustomNoJsModernStyle.CustomModern, c => c.DocumentTitle = "Sample Title");
    app.UseSwaggerUi(ModernStyle.Dark, c =>
    {
        c.DocumentTitle = "Sample Title";
        c.EnableAllAdvancedOptions();
        //c.EnablePinnedTopbar();
        //c.ShowBackToTopButton();
        //c.EnableStickyOperations();
        //c.EnableExpandOrCollapseAllOperations();
    });
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();