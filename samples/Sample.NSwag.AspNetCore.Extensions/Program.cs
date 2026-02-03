var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.Title = "Sample API - Extensions Demo";
    c.Version = "v1";

    // API Counter Extension
    // Automatically appends operation count to tag descriptions
    c.AppendOperationCountToTags();

    // Or with custom template:
    // c.AppendOperationCountToTags(" [{0} endpoints]");
    // c.AppendOperationCountToTags(" (APIs: {0})");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();