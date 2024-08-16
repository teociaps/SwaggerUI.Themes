using Sample.AspNetCore.SwaggerUI.NSwag;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddOpenApiDocument(OpenApiDocGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi("body { background-color: red; }", c => c.DocumentTitle = "Sample Title");
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();