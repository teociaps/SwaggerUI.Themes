using AspNetCore.Swagger.Themes;

namespace SwaggerThemes.Custom;

public class CustomTheme : Theme
{
    protected CustomTheme(string fileName) : base(fileName)
    {
    }

    public static CustomTheme Custom => new("custom.css");
}