# AspNetCore.SwaggerUI.Extensions

### Enhance your ASP.NET Core Swagger UI with functional extensions that go beyond theming!

**Automatic operation counts** &nbsp;•&nbsp; **Zero-config setup** • _**...and more coming soon!**_

**[Get Started](https://github.com/teociaps/SwaggerUI.Themes/wiki/Getting-Started)** • **[Full Documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)**

## 🚀 Quick Start

```bash
dotnet add package AspNetCore.SwaggerUI.Extensions
```

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AppendOperationCountToTags();
});
```

> **Note**: `AddSwaggerGen()` is provided by Swashbuckle.AspNetCore. This package adds convenient extension methods on top of it.

## ✨ Features

- **API Counter** - Automatically display operation counts in tag descriptions
- _...more extensions coming soon!_

## 📚 Basic Usage Examples

```csharp
builder.Services.AddSwaggerGen(c =>
{
    // Append operation count to tag descriptions
    c.AppendOperationCountToTags();

    // Or with a custom template
    c.AppendOperationCountToTags(" [{0} endpoints]");
});
```

## Related Packages

- **[AspNetCore.SwaggerUI.Themes](https://www.nuget.org/packages/AspNetCore.SwaggerUI.Themes/)** - Runtime theme switching and customization for Swashbuckle
- **[NSwag.AspNetCore.Extensions](https://www.nuget.org/packages/NSwag.AspNetCore.Extensions/)** - Extensions for NSwag users

---

#### Discover all the features and customization options in the [documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)!