# AspNetCore.SwaggerUI.Extensions

Enhance your ASP.NET Core Swagger UI with functional extensions that go beyond theming.

## Features

- **API Counter** - Automatically display operation counts in tag descriptions
- More extensions coming soon!

## Installation

```bash
dotnet add package AspNetCore.SwaggerUI.Extensions
```

## Quick Start

### API Counter

Add operation counts to your Swagger tags:

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AppendOperationCountToTags();
    // Or with custom template:
    c.AppendOperationCountToTags(" [{0} endpoints]");
});
```

This will append the operation count to each tag's description, helping users understand the API surface area at a glance.

## Documentation

For more information and advanced usage, visit the [Wiki](https://github.com/teociaps/SwaggerUI.Themes/wiki).

## Related Packages

- **AspNetCore.SwaggerUI.Themes** - Runtime theme switching and customization for Swashbuckle
- **NSwag.AspNetCore.Extensions** - Extensions for NSwag users

## License

MIT Licensed - see [LICENSE](https://github.com/teociaps/SwaggerUI.Themes/blob/main/LICENSE) for details.
