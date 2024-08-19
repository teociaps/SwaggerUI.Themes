using Models;

#if NET6_0

using NSwag.Annotations;

#endif

namespace Sample.AspNetCore.SwaggerUI.NSwag;

internal static class Endpoints
{
    internal static WebApplication AddEndpoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast", () =>
            {
                return Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        WeatherForecast.Summaries[Random.Shared.Next(WeatherForecast.Summaries.Length)]
                    ))
                    .ToArray();
            })
            .WithName("GetWeatherForecast")
            .WithInfo();

        app.MapPost("/weatherforecast/create", (WeatherForecast model) => model)
            .WithName("PostWeatherForecast")
            .WithInfo();

        app.MapPut("/weatherforecast/update", (int id, WeatherForecast model) => (id, model))
            .WithName("PutWeatherForecast")
            .WithInfo();

        app.MapDelete("/weatherforecast/delete", (int id) => id)
            .WithName("DeleteWeatherForecast")
            .WithInfo();

        return app;
    }

    private static RouteHandlerBuilder WithInfo(this RouteHandlerBuilder builder)
    {
        return builder
#if NET7_0_OR_GREATER
            .WithSummary("Summary")
            .WithDescription("Description Test")
            .WithOpenApi();
#else
            .WithMetadata(new OpenApiOperationAttribute(summary: "Summary", description: "Description Test"));
#endif
    }
}