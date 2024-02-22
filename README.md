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

- _New Themes_: enhances the Swagger documentation interface with different themes, including a default style that retains the classic Swagger UI appearance and new modern styles.
- _Seamless Integration_: simply install the package and add the style parameter to the existing method used for SwaggerUI.

## Themes

There are a few themes available for your Swagger UI:

### Default Styles

- __Dark__: offers a simple dark-themed interface, maintaining the classic Swagger UI layout;
- __Forest__: inspired by the colors of a forest, this theme brings a natural and vibrant feel to your documentation;
- __DeepSea__: inspired by the depths of the sea, this theme features cool blues and deep greens for a tranquil and immersive experience.

> The light style is not in this list because it's just the default one used by Swagger UI; to use that you don't need this library.

### Modern Styles

- __Light__: offers a modern, light-themed interface that overrides some aspects of the default Swagger UI;
- __Dark__: provides a sleek, dark-themed interface for a more modern look and feel.

## Supported .NET Versions

| Version | Status        |
| ------- | ------------- |
| .NET 6  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 7  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |
| .NET 8  | ![Badge](https://img.shields.io/badge/Status-Supported-brightgreen) |

### Version History

The table below provides a quick overview of **AspNetCore.SwaggerUI.Themes** versions and their compatibility with different .NET versions.

| Library Version | .NET 6 | .NET 7 | .NET 8 |
| --------------- | ------ | ------ | ------ |
| 0.1.0           | ❌	   | ❌		| ✔		 |
| 0.2.0 +         | ✔	   | ✔		| ✔		 |

- ✔️ Supported: The library version is compatible with the respective .NET version.
- ❌ Unsupported: The library version is not compatible with the respective .NET version.


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

> Please note that using the `InjectStylesheet()` method in the Swagger UI configuration will override the provided style.

## Contributing
If you have any suggestions, bug reports, or contributions, feel free to open an [issue](https://github.com/teociaps/SwaggerUI.Themes/issues) or submit a [pull request](https://github.com/teociaps/SwaggerUI.Themes/pulls)