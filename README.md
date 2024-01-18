# SwaggerUI.Themes

TODO: add widgets like build status and nuget version

**AspNetCore.SwaggerUI.Themes** is a package that extends [Swashbuckle.AspNetCore.SwaggerUI](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) to provide new themes for Swagger documentation in ASP.NET Core applications.

## Introduction

[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) is a popular library for adding Swagger support to ASP.NET Core projects, making it easier to document and interact with your APIs.

**AspNetCore.SwaggerUI.Themes** builds upon Swashbuckle.AspNetCore.SwaggerUI, enhancing the Swagger UI with modern and visually appealing themes.

## Features

- **New Themes:** Enhances the Swagger documentation interface with a modern themes, including dark theme.
- **Seamless Integration:** Simply install the package and add the style parameter to the existing method used for SwaggerUI.

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

2. In your `Program.cs` file, add the style through the `Style` class as new parameter of `app.UseSwaggerUI()` method:

	```csharp
	using AspNetCore.SwaggerUI.Themes;

	...

	app.UseSwaggerUI(Style.Dark);
	```
	
	This code enables the chosen theme for Swagger UI in your application.


NB: Only available style for now is the _Dark_ style. Other styles coming soon.

TODO: write better, for old templates too (startup/program)

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
app.UseSwaggerUI(Style.Dark, c =>
{
    // Your Swagger UI configuration here (optional)
});

...
```

That's it! Your Swagger UI will now have a sleek dark theme.

TODO: info override theme

## Contributing
If you have any suggestions, bug reports, or contributions, feel free to open an issue or submit a pull request.

TODO: write better