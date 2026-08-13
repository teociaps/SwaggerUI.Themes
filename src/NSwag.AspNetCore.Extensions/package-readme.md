# NSwag.AspNetCore.Extensions

### Enhance your ASP.NET Core Swagger UI with functional extensions that go beyond theming!

**Automatic operation counts** &nbsp;•&nbsp; **Zero-config setup** • _**...and more coming soon!**_

**[Get Started](https://github.com/teociaps/SwaggerUI.Themes/wiki/Getting-Started)** • **[Full Documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)**

## 🚀 Quick Start

```bash
dotnet add package NSwag.AspNetCore.Extensions
```

```csharp
builder.Services.AddOpenApiDocument(c =>
{
    c.AppendOperationCountToTags();
});
```

> **Note**: `AddOpenApiDocument()` is provided by NSwag.AspNetCore. This package adds convenient extension methods on top of it.

## ✨ Features

- **API Counter** - Automatically display operation counts in tag descriptions
- _...more extensions coming soon!_

## 📚 Basic Usage Examples

```csharp
builder.Services.AddOpenApiDocument(c =>
{
    // Append operation count to tag descriptions
    c.AppendOperationCountToTags();

    // Or with a custom template
    c.AppendOperationCountToTags(" [{0} endpoints]");
});
```

## Related Packages

- **[NSwag.AspNetCore.Themes](https://www.nuget.org/packages/NSwag.AspNetCore.Themes/)** - Runtime theme switching and customization for NSwag
- **[AspNetCore.SwaggerUI.Extensions](https://www.nuget.org/packages/AspNetCore.SwaggerUI.Extensions/)** - Extensions for Swashbuckle users

---

#### Discover all the features and customization options in the [documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)!