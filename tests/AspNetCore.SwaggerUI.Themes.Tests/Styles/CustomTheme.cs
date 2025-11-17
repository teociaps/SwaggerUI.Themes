namespace AspNetCore.Swagger.Themes.Tests.Styles;

public class CustomTheme : Theme
{
    protected CustomTheme(string fileName) : base(fileName)
    {
    }

    public static CustomTheme Custom => new("custom.css");
}