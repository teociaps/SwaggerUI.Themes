# NSwag.AspNetCore.Themes

**NSwag.AspNetCore.Themes** builds upon NSwag.AspNetCore, enhancing the Swagger UI with modern and visually appealing themes.


## Getting Started

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
	
> Setting the `CustomInlineStyles` property while configuring the NSwag settings will override the provided style.
> See [here](https://github.com/teociaps/SwaggerUI.Themes?tab=readme-ov-file#custom-styles) how to inject custom styles.

## Available Themes

See [here](https://github.com/teociaps/SwaggerUI.Themes?tab=readme-ov-file#available-themes) the available themes.