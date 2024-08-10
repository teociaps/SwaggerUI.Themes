#pragma warning disable S1133 // Deprecated code should be removed - Done for testing purposes

using System.ComponentModel.DataAnnotations;

namespace Models;

[Obsolete("test")]
public record Sample(WeatherForecast WeatherForecast, [Required] double Value);