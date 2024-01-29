using AspNetCore.SwaggerUI.Themes;
#if NET6_0 || NET7_0
using Swashbuckle.AspNetCore.Annotations;
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#if NET7_0_OR_GREATER
builder.Services.AddSwaggerGen();
#else
builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(Style.Dark, c => c.DocumentTitle = "Prova");
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
#if NET7_0_OR_GREATER
.WithSummary("Summary")
.WithDescription("Description Test")
.WithOpenApi();
#else
.WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Description Test"));
#endif

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}