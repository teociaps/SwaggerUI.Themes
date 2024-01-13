# AspNetCore.SwaggerUI.DarkTheme

TODO: add widgets like build status and nuget version

**AspNetCore.SwaggerUI.DarkTheme** is a package that extends [Swashbuckle.AspNetCore.SwaggerUI](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) to provide a sleek dark theme for Swagger documentation in ASP.NET Core applications.

## Introduction

[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) is a popular library for adding Swagger support to ASP.NET Core projects, making it easier to document and interact with your APIs.

**AspNetCore.SwaggerUI.DarkTheme** builds upon Swashbuckle.AspNetCore.SwaggerUI, enhancing the Swagger UI with a modern and visually appealing dark theme.

## Features

- **Dark Theme:** Enhances the Swagger documentation interface with a stylish dark theme.
- **Seamless Integration:** Simply install the package and use a single line of code to enable the dark theme for Swagger UI.

## Getting Started

To use **AspNetCore.SwaggerUI.DarkTheme** in your ASP.NET Core project, follow these steps:

1. Install the package using .NET CLI or NuGet Package Manager:

	```bash
	dotnet add package AspNetCore.SwaggerUI.DarkTheme
	```

	or

	```bash
	Install-Package AspNetCore.SwaggerUI.DarkTheme
	```

2. In your `Program.cs` file, add the following line instead `app.UseSwaggerUI()` method:

	```csharp
	app.UseDarkSwaggerUI();
	```
	
	This line enables the dark theme for Swagger UI in your application.

TODO: write better, for old templates too (startup/program)

## Example
Here's an example of how to integrate AspNetCore.SwaggerUI.DarkTheme in your ASP.NET Core application:

```csharp
...

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

...

// Use Swagger middleware
app.UseSwagger();

// Enable the dark theme for Swagger UI
app.UseDarkSwaggerUI(c =>
{
    // Your Swagger UI configuration here
});

...

```

That's it! Your Swagger UI will now have a sleek dark theme.

TODO: instructions to overwrite dark theme

## Contributing
If you have any suggestions, bug reports, or contributions, feel free to open an issue or submit a pull request.

TODO: write better