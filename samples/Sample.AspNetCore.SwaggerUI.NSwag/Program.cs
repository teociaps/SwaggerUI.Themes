using AspNetCore.Swagger.Themes;
using Sample.AspNetCore.SwaggerUI.NSwag;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddOpenApiDocument(OpenApiDocGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    //app.UseSwaggerUi(CustomModernStyle.CustomModern, c => c.DocumentTitle = "Sample Title");
    //app.UseSwaggerUi(Assembly.GetExecutingAssembly(), "modern.custom.css", c =>
    app.UseSwaggerUi(ModernStyle.Dark, c =>
    {
        c.DocumentTitle = "Sample Title";
        c.EnableAllAdvancedOptions();
        //c.EnablePinnableTopbar();
        //c.ShowBackToTopButton();
        //c.EnableStickyOperations();
        //c.EnableExpandOrCollapseAllOperations();
    });
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();