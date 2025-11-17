using AspNetCore.Swagger.Themes;

namespace SwaggerThemes;

public class CustomStyle : Style
{
    protected CustomStyle(string fileName) : base(fileName)
    {
    }

    public static CustomStyle Custom => new("custom.css");
}

public class CustomMinifiedStyle : Style
{
    protected CustomMinifiedStyle(string fileName, bool useMinified) : base(fileName, useMinified)
    {
    }

    public static CustomMinifiedStyle CustomMin => new("minifiedCustom.css", true);
}