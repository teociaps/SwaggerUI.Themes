<div align="center">

<p>
<img height="80" src="build/icon.png" alt="SwaggerUI.Themes Logo">
</p>

# SwaggerUI.Themes

<p>
    <a href="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/build.yml">
        <img alt="Build" src="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/build.yml/badge.svg" />
    </a>
    <a href="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/test.yml">
        <img alt="Tests" src="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/test.yml/badge.svg" />
    </a>
</p>

### Beautiful, modern themes for Swagger/OpenAPI documentation in ASP.NET Core

Make your API documentation look great with themes that fit your style.

**[Get Started](https://github.com/teociaps/SwaggerUI.Themes/wiki/Getting-Started)** • **[View Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Predefined-Themes)** • **[Full Documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)**

---

| Package | Purpose | NuGet |
|---------|---------|-------|
| **AspNetCore.SwaggerUI.Themes** | For [Swashbuckle.AspNetCore][swashbuckle-link] | [![swashbuckle-nuget]][swashbuckle-nuget-link] |
| **NSwag.AspNetCore.Themes** | For [NSwag.AspNetCore][nswag-link] | [![nswag-nuget]][nswag-nuget-link] |

---

</div>

> [!WARNING]
> **Version 3.0.0 Breaking Changes**
>
> Upgrading from v2.0.0? Please review the **[Migration Guide](https://github.com/teociaps/SwaggerUI.Themes/wiki/Migration-v3)** for important API changes.


## 🚀 Quick Start

```bash
# Install package
dotnet add package AspNetCore.SwaggerUI.Themes
# or
dotnet add package NSwag.AspNetCore.Themes
```

```csharp
// Apply a theme - that's it!
app.UseSwaggerUI(Theme.Dark);      // Swashbuckle
// or
app.UseSwaggerUi(Theme.Dark);      // NSwag
```

## ✨ Features

- **[Built-in Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Predefined-Themes)** - Choose from predefined themes ready to use

- **[Custom Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Custom-Themes)** - Build your own themes with full control, or create standalone themes with zero dependencies

- **[Advanced Features](https://github.com/teociaps/SwaggerUI.Themes/wiki/Advanced-Options)** - Enhance your documentation with new capabilities

**Discover more in the [Wiki](https://github.com/teociaps/SwaggerUI.Themes/wiki)!**

## 💡 Basic Usage Examples

### Swashbuckle

```csharp
using AspNetCore.Swagger.Themes;

...

// Simple
app.UseSwaggerUI(Theme.Dark);

// Or with advanced options
app.UseSwaggerUI(Theme.Dark, c =>
{
    c.EnableAllAdvancedOptions();
});

...
```

### NSwag

```csharp
using AspNetCore.Swagger.Themes;

...

// Simple
app.UseSwaggerUi(Theme.Dark);

// Or with advanced options
app.UseSwaggerUi(Theme.Dark, c =>
{
    c.EnableAllAdvancedOptions();
});

...
```

### Custom Theme

```csharp
public class MyTheme : Theme
{
    protected MyTheme() : base("my-theme.css") { }
    public static MyTheme Custom => new();
}

// Usage
app.UseSwaggerUI(MyTheme.Custom); // Swashbuckle

app.UseSwaggerUi(MyTheme.Custom); // NSwag
```

**Learn more in the [Wiki](https://github.com/teociaps/SwaggerUI.Themes/wiki)!**

## 🤝 Contributing

Contributions are welcome! See the [Contributing Guide](CONTRIBUTING.md) for details.

## 📜 License

MIT Licensed - see [LICENSE](LICENSE) for details.

---

<div align="center">

**Made with ❤️ by [@teociaps](https://github.com/teociaps)**

</div>

<!-- Links -->
[swashbuckle-nuget]: https://img.shields.io/nuget/v/AspNetCore.SwaggerUI.Themes?logo=nuget&label=Version&color=blue
[swashbuckle-nuget-link]: https://www.nuget.org/packages/AspNetCore.SwaggerUI.Themes/
[nswag-nuget]: https://img.shields.io/nuget/v/NSwag.AspNetCore.Themes?logo=nuget&label=Version&color=blue
[nswag-nuget-link]: https://www.nuget.org/packages/NSwag.AspNetCore.Themes/
[swashbuckle-link]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
[nswag-link]: https://github.com/RicoSuter/NSwag