# AspNetCore.NSwag.Themes

**AspNetCore.NSwag.Themes** builds upon NSwag.AspNetCore, enhancing the Swagger UI with modern and visually appealing themes.


## Getting Started

To use **AspNetCore.NSwag.Themes** in your ASP.NET Core project, follow these steps:

1. Install the package using .NET CLI or NuGet Package Manager:

	```bash
	dotnet add package AspNetCore.NSwag.Themes
	```

	or

	```bash
	Install-Package AspNetCore.NSwag.Themes
	```

2. In your `Program.cs` file, add the style through the `Style` or `ModernStyle` class as new parameter of `app.UseSwaggerUi()` method:

	```csharp
	using AspNetCore.Swagger.Themes;

	...

	app.UseSwaggerUi(Style.Dark);
	```

	This code enables the chosen theme for Swagger UI in your application.

> [!NOTE]
> Setting the `CustomInlineStyles` property while configuring the NSwag settings will override the provided style.
> See [here](https://github.com/teociaps/SwaggerUI.Themes?tab=readme-ov-file#custom-styles) how to inject custom styles.

## Available Themes

See [here](https://github.com/teociaps/SwaggerUI.Themes?tab=readme-ov-file#available-themes) the available themes.