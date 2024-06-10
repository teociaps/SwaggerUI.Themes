# AspNetCore.SwaggerUI.Themes

[![Build](https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/build.yml/badge.svg)](https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/build.yml) [![Test](https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/test.yml/badge.svg)](https://github.com/teociaps/SwaggerUI.Themes/actions/workflows/test.yml)

## Introduction

[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) is a popular library for adding Swagger support to ASP.NET Core projects, making it easier to document and interact with your APIs.

**AspNetCore.SwaggerUI.Themes** builds upon Swashbuckle.AspNetCore.SwaggerUI, enhancing the Swagger UI with modern and visually appealing themes.


## Features

- _New Themes_: enhances the Swagger documentation interface with various themes, including a default style that preserves the classic Swagger UI appearance and introduces new modern styles.
- _Seamless Integration_: simply install the package and add the style parameter to the existing method used for SwaggerUI.

> Modern styles come with additional functionalities, including _**pinned topbar**_ and _**back-to-top button**_.


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


## Available Themes
There are a few styles available for your Swagger UI.

### Defaults:

### • Dark
Offers a simple dark-themed interface, maintaining the classic Swagger UI layout.

![dark style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-dark.png)

```
Style.Dark
```

### • Forest
Inspired by the colors of a forest, this theme brings a natural and vibrant feel to your documentation.

![forest style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-forest.png)

```
Style.Forest
```

### • DeepSea
Inspired by the depths of the sea, this theme features cool blues and deep greens for a tranquil and immersive experience.

![deep sea style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-deepsea.png)

```
Style.DeepSea
```

### • Desert
Inspired by the essence of the desert, this theme evokes warm tones and sandy hues for an immersive experience.

![desert style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/default-desert.png)

```
Style.Desert
```

> The light style is not in this list because it's just the default one used by Swagger UI; to use that you don't need this library.


### Moderns:

### • Light
Offers a modern, light-themed interface that overrides some aspects of the default Swagger UI.

![modern dark style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-light.png)

```
ModernStyle.Light
```

### • Dark
Provides a sleek, dark-themed interface for a more modern look and feel.

![modern dark style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-dark.png)

```
ModernStyle.Dark
```

### • Forest
Brings a natural feel to your documentation with colors inspired by the serene ambiance of a forest.

![modern forest style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-forest.png)

```
ModernStyle.Forest
```

### • DeepSea
Dive into a tranquil interface with cool blues and deep greens, inspired by the depths of the sea.

![modern deep sea style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-deepsea.png)

```
ModernStyle.DeepSea
```

### • Desert
Immerse yourself in a desert-inspired theme, characterized by warm tones and sandy hues.

![modern desert style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-desert.png)

```
ModernStyle.Desert
```

### • Futuristic
Experience a futuristic neon-inspired interface for a modern browsing experience.

![modern futuristic style example image](https://raw.githubusercontent.com/teociaps/SwaggerUI.Themes/main/samples/screenshots/modern-futuristic.png)

```
ModernStyle.Futuristic
```