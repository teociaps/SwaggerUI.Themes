using AspNetCore.Swagger.Themes;

namespace CompanyThemes;

/// <summary>
/// Company brand themes for Swagger UI.
/// </summary>
public class CompanyThemes : Theme
{
    protected CompanyThemes(string fileName) : base(fileName)
    {
    }

    public static CompanyThemes CorporateBlue => new("corporate-blue.css");

    public static CompanyThemes TechGreen => new("tech-green.css");

    public static CompanyThemes StartupPurple => new("startup-purple.css");
}