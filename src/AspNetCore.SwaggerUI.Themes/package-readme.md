<div align="center">

<p>
<img height="50" src="https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/refs/heads/main/build/icon.png" alt="SwaggerUI.Themes Logo">
</p>

# AspNetCore.SwaggerUI.Themes

### Beautiful, modern themes for Swashbuckle Swagger UI in ASP.NET Core

Make your API documentation look great with themes that fit your style.

**[Get Started](https://github.com/teociaps/SwaggerUI.Themes/wiki/Getting-Started)** • **[View Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Predefined-Themes)** • **[Documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)**

</div>

> ⚠️ **Version 3.0 Breaking Changes**  
> Upgrading from v2.0.0? Please review the **[Migration Guide](https://github.com/teociaps/SwaggerUI.Themes/wiki/Migration-v3)** for important API changes.

## Installation

```bash
dotnet add package AspNetCore.SwaggerUI.Themes
```

## Quick Start

```csharp
app.UseSwaggerUI(Theme.Dark);
```

> **Note**: The `UseSwaggerUI()` method is provided by Swashbuckle.AspNetCore. This package adds convenient overloads to apply themes seamlessly.

## Features

- **[Built-in Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Predefined-Themes)** - Predefined themes ready to use

- **[Advanced Features](https://github.com/teociaps/SwaggerUI.Themes/wiki/Advanced-Options)** - Unlock new capabilities to enhance your Swagger UI

- **[Custom Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Custom-Themes)** - Create your own themes with full control or build standalone themes

## Usage

```csharp
using AspNetCore.Swagger.Themes;

...

// Use a built-in theme
app.UseSwaggerUI(Theme.Dark);

// Or with advanced features
app.UseSwaggerUI(Theme.Dark, c =>
{
    c.EnableAllAdvancedOptions();
});

// Or use your custom theme from assembly
app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "my-theme.css");

...
```

---

#### Discover all the features and customization options in the [documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)!