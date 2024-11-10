<span align="center">

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
> Starting from v1.0.0 the namespace for pre-defined styles is `AspNetCore.Swagger.Themes` instead of `AspNetCore.SwaggerUI.Themes`!


## Features
- _New Themes_: enhances the Swagger documentation interface with various themes, including a default style that preserves the classic Swagger UI appearance and introduces new modern styles. Explore samples [here](#available-themes).
- _Seamless Integration_: simply install the package and add the style parameter to the existing method used for Swagger UI.
- 🆕 _Custom Styles_: create your own style for Swagger documentation. You can either inherit from the classic or modern common styles, or define a completely new style (see more [here](#custom-styles)).


## Supported .NET Versions
| Version | Status |
| ------- | ------ |
| .NET 6  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 7  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 8  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 9  | ![Badge](https://img.shields.io/badge/Status-Coming%20soon...-blue) |


## Swashbuckle.AspNetCore.SwaggerUI
Customize the Swashbuckle API documentation UI by using **AspNetCore.SwaggerUI.Themes** in your ASP.NET Core project:

1. Install the package using .NET CLI or NuGet Package Manager:

	```bash
	dotnet add package AspNetCore.SwaggerUI.Themes
	```

	or

	```bash
	Install-Package AspNetCore.SwaggerUI.Themes
	```

2. In your `Program.cs` file, add the style through the `Style`, `ModernStyle` or `NoJsModernStyle` class as new parameter of `app.UseSwaggerUI()` method:

	```csharp
	using AspNetCore.Swagger.Themes;

	...

	app.UseSwaggerUI(ModernStyle.Dark, options => ...);
	```

	This code enables the chosen theme for Swagger UI in your application.
	
> [!NOTE]
> Using the `InjectStylesheet()` method in the Swagger UI configuration will override the provided style.
> See [here](#custom-styles) how to inject custom styles.


## NSwag.AspNetCore
Customize the NSwag API documentation UI by using **NSwag.AspNetCore.Themes** in your ASP.NET Core project:

1. Install the package using .NET CLI or NuGet Package Manager:

	```bash
	dotnet add package NSwag.AspNetCore.Themes
	```

	or

	```bash
	Install-Package NSwag.AspNetCore.Themes
	```

2. In your `Program.cs` file, add the style through the `Style`, `ModernStyle` or `NoJsModernStyle` class as new parameter of `app.UseSwaggerUi()` method:

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

> The light style is not in this list because it's just the default one used by Swagger UI; to use that you don't need this library.

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
> Opt for Modern Styles! Modern styles come with additional functionalities, including _**pinned topbar**_ and _**back-to-top button**_.

🆕You can also choose for a version without additional JavaScript if desired by using the pre-built `NoJsModernStyle` class.

> [!NOTE]
> The classic and modern **dark styles** will only load if your browser's color scheme preference is set to _dark_; otherwise, the light style is loaded.


## 🆕Custom Styles
You can customize the Swagger UI in your ASP.NET Core application by applying custom CSS styles.
Here are the available methods.

### Inline CSS Styles
You can directly write your custom CSS content as a string and apply it to the Swagger UI:

```csharp
var cssContent = "body { background-color: #f5f5f5; }";

// Swashbuckle
app.UseSwaggerUI(cssContent, options => ...);
	
// NSwag
app.UseSwaggerUi(cssContent, settings => ...);
```

### Applying Embedded CSS Files from Assemblies
To apply a CSS file that is embedded as a resource within an assembly, place the file in a folder named **"SwaggerThemes"** inside the assembly.
Then, specify the CSS filename and the assembly where the file is located:
	
```csharp
var assembly = Assembly.GetExecutingAssembly();
var cssFileName = "myCustomStyle.css"; // Only the filename, no need to specify the "SwaggerThemes" folder

// Swashbuckle
app.UseSwaggerUI(assembly, cssFileName, options => ...);
	
// NSwag
app.UseSwaggerUi(assembly, cssFileName, settings => ...);
```
	
> [!TIP]
> If your CSS file's name starts with **"classic."** or **"modern."**, the method automatically prepends a related common style (either classic or modern) to your custom styles.
> These common styles serve as the base for [pre-defined styles](#available-themes) that enhance the Swagger UI.

### Creating Custom Styles by Inheriting from Base Classes
Another powerful customization option is to create your own style classes by inheriting from the `Style`, `ModernStyle` or `NoJsModernStyle` base classes.
This approach allows you to define new styles that automatically incorporate common base styles and, for modern themes, additional JavaScript.

Here’s how to create a custom style:

```csharp
// Use modern style loading additional JS
public class CustomModernStyle : ModernStyle
{
	protected CustomModernStyle(string fileName) : base(fileName)
	{
	}

	public static CustomModernStyle CustomModern => new("modern.custom.css");
}

// Use modern style without loading additional JS
public class CustomNoJsModernStyle : NoJsModernStyle
{
	protected CustomNoJsModernStyle(string fileName) : base(fileName)
	{
	}

	public static CustomNoJsModernStyle CustomModern => new("modern.custom.css");
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
> Custom styles that inherit from the `ModernStyle` class include additional JavaScript.
> This doesn't apply to styles that inherit from `NoJsModernStyle`, inline styles, or those applied directly from assemblies.


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


[Swashbuckle Nuget Version]: https://img.shields.io/nuget/v/AspNetCore.SwaggerUI.Themes?logo=nuget&label=AspNetCore.SwaggerUI.Themes&color=blue
[NSwag Nuget Version]: https://img.shields.io/nuget/v/NSwag.AspNetCore.Themes?logo=nuget&label=NSwag.AspNetCore.Themes&color=blue

[Swashbuckle.AspNetCore.SwaggerUI]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
[NSwag.AspNetCore]: https://github.com/RicoSuter/NSwag?tab=readme-ov-file#aspnet-and-aspnet-core
