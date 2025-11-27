using AspNetCore.Swagger.Themes;

namespace SwaggerThemes.Custom;

public class CustomMinifiedStyle : Theme
{
    protected CustomMinifiedStyle(string fileName, bool useMinified) : base(fileName, useMinified)
    {
    }

    public static CustomMinifiedStyle CustomMin => new("minifiedCustom.css", true);
}