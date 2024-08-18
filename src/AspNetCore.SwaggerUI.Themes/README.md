> **Warning**: From v1.0.0 the namespace of pre-defined styles is `AspNetCore.Swagger.Themes`, instead of `AspNetCore.SwaggerUI.Themes`!


# AspNetCore.SwaggerUI.Themes

**AspNetCore.SwaggerUI.Themes** builds upon Swashbuckle.AspNetCore.SwaggerUI, enhancing the Swagger UI with modern and visually appealing themes.


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
	using AspNetCore.Swagger.Themes;

	...

	app.UseSwaggerUI(Style.Dark);
	```

	This code enables the chosen theme for Swagger UI in your application.

> [!NOTE]
> Using the `InjectStylesheet()` method in the Swagger UI configuration will override the provided style.
> See [here](https://github.com/teociaps/SwaggerUI.Themes?tab=readme-ov-file#custom-styles) how to inject custom styles.

## Available Themes

See [here](https://github.com/teociaps/SwaggerUI.Themes?tab=readme-ov-file#available-themes) the available themes.