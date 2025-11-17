using AspNetCore.Swagger.Themes;

namespace SwaggerThemes;

public class CustomTheme : Theme
{
    protected CustomTheme(string fileName) : base(fileName)
    {
    }

    public static CustomTheme Custom => new("custom.css");
}

public class CustomMinifiedStyle : Theme
{
    protected CustomMinifiedStyle(string fileName, bool useMinified) : base(fileName, useMinified)
    {
    }

    public static CustomMinifiedStyle CustomMin => new("minifiedCustom.css", true);
}