using AspNetCore.Swagger.Themes;

namespace SwaggerThemes;

public class CustomNoJsModernStyle : NoJsModernStyle
{
    protected CustomNoJsModernStyle(string fileName) : base(fileName)
    {
    }

    public static CustomNoJsModernStyle CustomModern => new("modern.custom.css");
}

public class CustomStyle : Style
{
    protected CustomStyle(string fileName) : base(fileName)
    {
    }

    public static CustomStyle Custom => new("classic.custom.css");
}