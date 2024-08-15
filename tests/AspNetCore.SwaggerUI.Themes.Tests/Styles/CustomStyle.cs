namespace AspNetCore.Swagger.Themes.Tests.Styles;

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

    public static CustomStyle Custom => new("custom.css");
}