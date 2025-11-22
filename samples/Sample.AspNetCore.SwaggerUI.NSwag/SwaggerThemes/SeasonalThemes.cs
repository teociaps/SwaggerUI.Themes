using AspNetCore.Swagger.Themes;

namespace SwaggerThemes;

/// <summary>
/// Seasonal themes for special occasions.
/// </summary>
public class SeasonalThemes : Theme
{
    protected SeasonalThemes(string fileName) : base(fileName)
    {
    }

    /// <summary>
    /// Festive theme for holidays.
    /// </summary>
    public static SeasonalThemes HolidayRed => new("holiday-red.css");

    /// <summary>
    /// Summer vacation theme.
    /// </summary>
    public static SeasonalThemes OceanBlue => new("ocean-blue.css");
}