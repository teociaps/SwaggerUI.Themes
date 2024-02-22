using AspNetCore.SwaggerUI.Themes;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

#if NET6_0 || NET7_0

using Swashbuckle.AspNetCore.Annotations;

#endif

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        #if NET6_0 || NET7_0
        options.EnableAnnotations();
        #endif
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ToDo API",
            Description = "An ASP.NET Core Web API for testing",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "Example License",
                Url = new Uri("https://example.com/license")
            }
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.
                          Enter 'Bearer' [space] and then your token in the text input below.
                          Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ModernStyle.Dark, c => c.DocumentTitle = "Sample Title");
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

app.MapPost("/weatherforecast/create", (WeatherForecast model) => model)
.WithName("PostWeatherForecast")
#if NET7_0_OR_GREATER
.WithSummary("Summary")
.WithDescription("Description Test")
.WithOpenApi();
#else
.WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Description Test"));
#endif

app.MapPut("/weatherforecast/update", (int id, WeatherForecast model) => (id, model))
.WithName("PutWeatherForecast")
#if NET7_0_OR_GREATER
.WithSummary("Summary")
.WithDescription("Description Test")
.WithOpenApi();
#else
.WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Description Test"));
#endif

app.MapDelete("/weatherforecast/delete", (int id) => id)
.WithName("DeleteWeatherForecast")
#if NET7_0_OR_GREATER
.WithSummary("Summary")
.WithDescription("Description Test")
.WithOpenApi();
#else
.WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Description Test"));
#endif

app.MapControllers();

app.Run();

public record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    [Obsolete("test")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

[Obsolete("test")]
public record Sample(WeatherForecast WeatherForecast, [Required] double Value);