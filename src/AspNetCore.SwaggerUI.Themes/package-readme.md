<div align="center">

<p>
<img height="50" src="https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/refs/heads/main/build/icon.png" alt="SwaggerUI.Themes Logo">
</p>

# AspNetCore.SwaggerUI.Themes

### Give your ASP.NET Core API documentation the look it deserves!

**Switch themes at runtime** &nbsp;•&nbsp; **Unlock new capabilities** &nbsp;•&nbsp; **Create and choose your custom style**
<br>
_**...and more!**_

**[Get Started](https://github.com/teociaps/SwaggerUI.Themes/wiki/Getting-Started)** • **[View Built-in Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Predefined-Themes)** • **[Full Documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)**

</div>

> ⚠️ **Version 3.0 Breaking Changes**  
> Upgrading from v2.0.0? Please review the **[Migration Guide](https://github.com/teociaps/SwaggerUI.Themes/wiki/Migration-v3)** for important API changes.

## 🚀 Quick Start

```bash
dotnet add package AspNetCore.SwaggerUI.Themes
```

```csharp
// Apply a theme
app.UseSwaggerUI(Theme.Dark);

// Or enable runtime theme switcher!
app.UseSwaggerUI(Theme.Dark, c => c.EnableThemeSwitcher());
```

> **Note**: The `UseSwaggerUI()` method is provided by Swashbuckle.AspNetCore. This package adds convenient overloads to apply themes seamlessly.

## ✨ Features

- 🔥 **[Theme Switcher](https://github.com/teociaps/SwaggerUI.Themes/wiki/Feature-Theme-Switcher)** - Switch built-in and custom themes dynamically without page reload

- **[Built-in Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Predefined-Themes)** - Choose from predefined themes ready to use

- **[Custom Themes](https://github.com/teociaps/SwaggerUI.Themes/wiki/Custom-Themes)** - Build your own themes with full control, or create standalone themes with zero dependencies

- **[Advanced Features](https://github.com/teociaps/SwaggerUI.Themes/wiki/Advanced-Options)** - Enhance your documentation with powerful UI capabilities

- _...discover more in the [Wiki](https://github.com/teociaps/SwaggerUI.Themes/wiki/Features)!_

## 📚 Basic Usage Examples

```csharp
using AspNetCore.Swagger.Themes;

...

// Use a built-in theme
app.UseSwaggerUI(Theme.Dark);

// Enable theme switcher with auto-discovery
app.UseSwaggerUI(Theme.Dark, c =>
{
    c.EnableThemeSwitcher();
});

// Or with all advanced features
app.UseSwaggerUI(Theme.Dark, c =>
{
    c.EnableAllAdvancedOptions();
});

// Or use your custom theme from assembly
app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "my-theme.css", c =>
{
    c.EnableThemeSwitcher(); // Works with custom themes too!
});

...
```

---

#### Discover all the features and customization options in the [documentation](https://github.com/teociaps/SwaggerUI.Themes/wiki)!