#pragma warning disable S1133 // Deprecated code should be removed - Done for testing purposes

namespace Models;

public record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    public static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [Obsolete("test")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}