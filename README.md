﻿<span align="center">

<p>
<img height="50" src="\build\icon.png" align="center">
</p>

# SwaggerUI.Themes

</span>
<p align="center">
    <a href="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/build.yml">
        <img alt="Build Passing" src="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/build.yml/badge.svg" />
    </a>
    <a href="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/test.yml">
        <img alt="Tests Passing" src="https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/test.yml/badge.svg" />
    </a>
</p>
<span align="center">

Change style to your API documentation in ASP.NET Core applications!

</span>
<hr>

| Package | Purpose |
| ------- | ------- |
| [![Swashbuckle Nuget Version]](https://www.nuget.org/packages/AspNetCore.SwaggerUI.Themes/) | Customize the style for [Swashbuckle.AspNetCore.SwaggerUI] |
| [![NSwag Nuget Version]](https://www.nuget.org/packages/NSwag.AspNetCore.Themes/) | Customize the style for [NSwag.AspNetCore] |


> [!WARNING]
> If you're upgrading from 1.0.0 to 2.x, there's a breaking change to be aware of: **NoJsModernStyle is no more available, use ModernStyle instead**. See the [release notes](https://github.com/teociaps/SwaggerUI.Themes/releases/tag/v2.0.0) for details.


## Features
- __[New Themes](#available-themes)__: Choose from a variety of themes to customize the Swagger documentation interface. Options include a default style that preserves the classic Swagger UI look, along with fresh, modern styles.
- 🆕 __[Advanced Options](#advanced-options)__: Access expanded features with both classic and modern styles for an optimized API documentation experience.
- __[Custom Styles](#custom-styles)__: Design your own Swagger UI style by either extending the classic or modern base styles or creating a completely new look.
- __[Easy Integration](#getting-started)__: and add style parameters to the existing Swagger UI setup for a seamless upgrade.


## Supported .NET Versions
| Version | Status |
| ------- | ------ |
| .NET 6  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 7  | ![Badge](https://img.shields.io/badge/Status-Out%20of%20Support*-orange) |
| .NET 8  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 9  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |

___* Still available but it won't receive any update. Upgrade your .NET version!___


## Getting Started

### Swashbuckle.AspNetCore.SwaggerUI
Customize your Swashbuckle API documentation UI by using **AspNetCore.SwaggerUI.Themes** in ASP.NET Core:

1. Install the package using .NET CLI or NuGet Package Manager:

    ```bash
    dotnet add package AspNetCore.SwaggerUI.Themes
    ```

    or

    ```bash
    Install-Package AspNetCore.SwaggerUI.Themes
    ```

2. In your `Program.cs` file, add the style through the `Style` or `ModernStyle` class as new parameter of `app.UseSwaggerUI()` method:

    ```csharp
    using AspNetCore.Swagger.Themes;

    ...

    app.UseSwaggerUI(ModernStyle.Dark, options => ...);
    ```

    This code enables the chosen theme for Swagger UI in your application.
    
> [!NOTE]
> Using the `InjectStylesheet()` method in the Swagger UI configuration will override the provided style.
> See [here](#custom-styles) how to inject custom styles.


### NSwag.AspNetCore
Customize your NSwag API documentation UI by using **NSwag.AspNetCore.Themes** in ASP.NET Core:

1. Install the package using .NET CLI or NuGet Package Manager:

    ```bash
    dotnet add package NSwag.AspNetCore.Themes
    ```

    or

    ```bash
    Install-Package NSwag.AspNetCore.Themes
    ```

2. In your `Program.cs` file, add the style through the `Style` or `ModernStyle` class as new parameter of `app.UseSwaggerUi()` method:

    ```csharp
    using AspNetCore.Swagger.Themes;

    ...

    app.UseSwaggerUi(ModernStyle.Dark, settings => ...);
    ```

    This code enables the chosen theme for Swagger UI in your application.
    
> [!NOTE]
> Setting the `CustomInlineStyles` property while configuring the NSwag settings will override the provided style.
> See [here](#custom-styles) how to inject custom styles.


## Available Themes
There are a few pre-defined styles available for your Swagger UI.

### Classics:

| Dark | Forest | DeepSea | Desert |
|------|--------|---------|--------|
| ![dark style example image] | ![forest style example image] | ![deepSea style example image] | ![desert style example image] |
| <center><pre lang="csharp">`Style.Dark`</pre></center> | <center><pre lang="csharp">`Style.Forest`</pre></center> | <center><pre lang="csharp">`Style.DeepSea`</pre></center> | <center><pre lang="csharp">`Style.Desert`</pre></center> |

> [!NOTE]
> The light style is not shown in this list because it's the default style used by Swagger UI. If you want to use this theme with [Advanced Options](#advanced-options) for the classic theme, you can opt for `Style.Light`.

### Moderns:

| Light | Dark | Forest |
|-------|------|--------|
| ![modern light style example image] | ![modern dark style example image] | ![modern forest style example image] |
| <center><pre lang="csharp">`ModernStyle.Light`</pre></center> | <center><pre lang="csharp">`ModernStyle.Dark`</pre></center> | <center><pre lang="csharp">`ModernStyle.Forest`</pre></center> |

| DeepSea | Desert | Futuristic |
|---------|--------|------------|
| ![modern deepSea style example image] | ![modern desert style example image] | ![modern futuristic style example image] |
| <center><pre lang="csharp">`ModernStyle.DeepSea`</pre></center> | <center><pre lang="csharp">`ModernStyle.Desert`</pre></center> | <center><pre lang="csharp">`ModernStyle.Futuristic`</pre></center> |

> [!TIP]
> Opt for Modern Styles! Modern styles offer additional functionalities. Explore [Advanced Options](#advanced-options) for enhanced customization.

> [!NOTE]
> The classic and modern **dark styles** will only load if your browser's color scheme preference is set to _dark_; otherwise, the light style is loaded.


## 🆕Advanced Options
Unlock new capabilities in your Swagger documentation with added features for both classic and modern themes, designed to improve navigation and usability.

### Both Classic and Modern Themes

* #### Sticky Operations
    Keeps the operations head panel in view as you scroll, making it easier to navigate large API documentation:
    ```csharp
    // Swashbuckle
    app.UseSwaggerUI(ModernStyle.Dark, options =>
    {
        options.EnableStickyOperations();
    });

    // NSwag
    app.UseSwaggerUi(ModernStyle.Dark, settings =>
    {
        settings.EnableStickyOperations();
    });
    ```

    ![sticky operations gif]

### Modern Themes

* #### Pinnable Topbar
    The top navigation bar remains fixed at the top of the page, so you can quickly access key options:
    ```csharp
    // Swashbuckle
    app.UseSwaggerUI(ModernStyle.Dark, options =>
    {
        options.EnablePinnableTopbar();
    });

    // NSwag
    app.UseSwaggerUi(ModernStyle.Dark, settings =>
    {
        settings.EnablePinnableTopbar();
    });
    ```

    ![pinnable topbar gif]

* #### Back to Top Button
    Easily return to the top of the documentation with a single click, especially helpful for long pages:
    ```csharp
    // Swashbuckle
    app.UseSwaggerUI(ModernStyle.Dark, options =>
    {
        options.ShowBackToTopButton();
    });

    // NSwag
    app.UseSwaggerUi(ModernStyle.Dark, settings =>
    {
        settings.ShowBackToTopButton();
    });
    ```

    ![back-to-top gif]

* #### Expand/Collapse All Operations
    Expand or collapse all operations within a tag group with a single click. This makes it easier to manage large sets of API operations and navigate more efficiently:
    ```csharp
    // Swashbuckle
    app.UseSwaggerUI(ModernStyle.Dark, options =>
    {
        options.EnableExpandOrCollapseAllOperations();
    });

    // NSwag
    app.UseSwaggerUi(ModernStyle.Dark, settings =>
    {
        settings.EnableExpandOrCollapseAllOperations();
    });
    ```

    ![expand-collapse all gif]

> [!NOTE]
> The features for modern themes require JavaScript to work.

To enable all these features simultaneously, use the `EnableAllAdvancedOptions()` method:
```csharp
// Swashbuckle
app.UseSwaggerUI(ModernStyle.Dark, options =>
{
    options.EnableAllAdvancedOptions();
});

// NSwag
app.UseSwaggerUi(ModernStyle.Dark, settings =>
{
    settings.EnableAllAdvancedOptions();
});
```

> [!NOTE]
> Features are automatically managed based on the chosen style, whether it is classic or modern.


## Custom Styles
Customize the Swagger UI in your ASP.NET Core application by applying custom CSS styles using various methods.

### Inline CSS Styles
Directly write your custom CSS content as a string and apply it to the Swagger UI:

```csharp
var cssContent = "body { background-color: #f5f5f5; }";

// Swashbuckle
app.UseSwaggerUI(cssContent, options => ...);
    
// NSwag
app.UseSwaggerUi(cssContent, settings => ...);
```

### Applying Embedded CSS Files from Assemblies
To apply a CSS file embedded as a resource within an assembly, place it in a folder named **"SwaggerThemes"** inside the assembly.
Specify the CSS filename (without the folder name) and the assembly where the file is located:
    
```csharp
var assembly = Assembly.GetExecutingAssembly();
var cssFileName = "myCustomStyle.css"; // Only the filename, no need to specify the path

// Swashbuckle
app.UseSwaggerUI(assembly, cssFileName, options => ...);
    
// NSwag
app.UseSwaggerUi(assembly, cssFileName, settings => ...);
```
    
> [!TIP]
> If your CSS file's name begins with **"classic."** or **"modern."**, a related common style (either classic or modern) will automatically be applied as a base for your custom styles.
> 
> - **"classic."** files use a classic common style.
> - **"modern."** files use a modern common style and can enable additional JavaScript functionalities.
>
> These common styles serve as the base for [pre-defined styles](#available-themes) that enhance the Swagger UI.

### Creating Custom Styles by Inheriting from Base Classes
Another powerful customization option is to create a style by inheriting from the `Style` or `ModernStyle` base classes.
This approach allows you to define new styles that automatically incorporate common base styles and, for modern themes, enable JavaScript-related features.

Here's how to create a custom style:

```csharp
// Use modern style with the possibility to enable additional JS features
public class CustomModernStyle : ModernStyle
{
    protected CustomModernStyle(string fileName) : base(fileName)
    {
    }

    public static CustomModernStyle CustomModern => new("modernCustom.css");
}

// Use classic style
public class CustomStyle : Style
{
    protected CustomStyle(string fileName) : base(fileName)
    {
    }

    public static CustomStyle Custom => new("custom.css");
}
```
    
Place your CSS file (e.g., "custom.css") as an embedded resource in the same assembly where your custom style class is located.
Then, apply it to the Swagger UI:

```csharp
var customStyle = CustomStyle.Custom;
    
// Swashbuckle
app.UseSwaggerUI(customStyle, options => ...);
    
// NSwag
app.UseSwaggerUi(customStyle, settings => ...);
```

> [!NOTE]
> Only _Inline CSS Styles_ do not support [Advanced Options](#advanced-options); **the other methods do**.

> [!IMPORTANT]
> Styles and JavaScript resources are cached by default.
> If you make changes to a style or encounter issues after modifying settings, try refreshing the cache in your browser or clearing local storage to ensure the latest version is loaded.


## Contributing
If you have any suggestions, bug reports, or contributions, feel free to open an [issue](https://github.com/teociaps/SwaggerUI.Themes/issues) or submit a [pull request](https://github.com/teociaps/SwaggerUI.Themes/pulls)





[Dark Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-dark.png
[Forest Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-forest.png
[DeepSea Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-deepsea.png
[Desert Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-desert.png

[Modern Light Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-light.png
[Modern Dark Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-dark.png
[Modern Forest Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-forest.png
[Modern DeepSea Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-deepsea.png
[Modern Desert Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-desert.png
[Modern Futuristic Style Example Image]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-futuristic.png

[Sticky Operations GIF]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/assets/sticky-operations.gif
[Pinnable Topbar GIF]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/assets/pinnable-topbar.gif
[Back-To-Top GIF]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/assets/back-to-top.gif
[Expand-Collapse All GIF]: https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/assets/expand-collapse-all-operations.gif

[Swashbuckle Nuget Version]: https://img.shields.io/nuget/v/AspNetCore.SwaggerUI.Themes?logo=nuget&label=AspNetCore.SwaggerUI.Themes&color=blue
[NSwag Nuget Version]: https://img.shields.io/nuget/v/NSwag.AspNetCore.Themes?logo=nuget&label=NSwag.AspNetCore.Themes&color=blue

[Swashbuckle.AspNetCore.SwaggerUI]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
[NSwag.AspNetCore]: https://github.com/RicoSuter/NSwag?tab=readme-ov-file#aspnet-and-aspnet-core
