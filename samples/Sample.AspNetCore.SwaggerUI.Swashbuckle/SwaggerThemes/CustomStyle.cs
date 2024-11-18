using AspNetCore.Swagger.Themes;

namespace SwaggerThemes;

public class CustomModernStyle : ModernStyle
{
    protected CustomModernStyle(string fileName) : base(fileName)
    {
    }

    public static CustomModernStyle CustomModern => new("modern.custom.css");
}

public class CustomStyle : Style
{
    protected CustomStyle(string fileName) : base(fileName)
    {
    }

    public static CustomStyle Custom => new("classic.custom.css");
}