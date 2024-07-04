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
	<a href="https://www.nuget.org/packages/AspNetCore.SwaggerUI.Themes">
		<img alt="NuGet Version" src="https://img.shields.io/nuget/v/AspNetCore.SwaggerUI.Themes?logo=nuget&color=blue" />
	</a>
</p>
<span align="center">

**AspNetCore.SwaggerUI.Themes** is a package that extends [Swashbuckle.AspNetCore.SwaggerUI](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) to provide new themes for Swagger documentation in ASP.NET Core applications.

</span>


## Introduction
[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) is a popular library for adding Swagger support to ASP.NET Core projects, making it easier to document and interact with your APIs.

**AspNetCore.SwaggerUI.Themes** builds upon Swashbuckle.AspNetCore.SwaggerUI, enhancing the Swagger UI with modern and visually appealing themes.


## Features
- _New Themes_: enhances the Swagger documentation interface with various themes, including a default style that preserves the classic Swagger UI appearance and introduces new modern styles. Explore samples [here](#available-themes).
- _Seamless Integration_: simply install the package and add the style parameter to the existing method used for SwaggerUI.

> [!TIP]
> Opt for Modern Styles! Modern styles come with additional functionalities, including _**pinned topbar**_ and _**back-to-top button**_.


## Supported .NET Versions
| Version | Status        |
| ------- | ------------- |
| .NET 6  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 7  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 8  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |


## Getting Started
To use **AspNetCore.SwaggerUI.Themes** in your ASP.NET Core project, follow these steps:

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
	using AspNetCore.SwaggerUI.Themes;

	...

	app.UseSwaggerUI(Style.Dark);
	```

	This code enables the chosen theme for Swagger UI in your application.

> [!NOTE]	
> Please be aware that for projects utilizing the older style template with separate `Startup.cs` and `Program.cs` files, the previously code should be configured within the `Configure` method of the `Startup` class.


## Example
Here's an example of how to integrate AspNetCore.SwaggerUI.Themes in your ASP.NET Core application:

```csharp
using AspNetCore.SwaggerUI.Themes;
...

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

...

// Use Swagger middleware
app.UseSwagger();

// Enable the dark theme for Swagger UI
app.UseSwaggerUI(ModernStyle.Dark, c =>
{
    // Your Swagger UI configuration here (optional)
});

...
```

That's it! Your Swagger UI will now have a sleek dark theme.

> [!IMPORTANT]
> Using the `InjectStylesheet()` method in the Swagger UI configuration will override the provided style.


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