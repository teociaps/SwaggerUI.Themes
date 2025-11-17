namespace AspNetCore.Swagger.Themes.Tests.Styles;

public class CustomStyle : Style
{
    protected CustomStyle(string fileName) : base(fileName)
    {
    }

    public static CustomStyle Custom => new("custom.css");
}