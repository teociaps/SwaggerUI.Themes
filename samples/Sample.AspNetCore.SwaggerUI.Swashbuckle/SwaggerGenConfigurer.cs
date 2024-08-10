#pragma warning disable S1075 // URIs should not be hardcoded - Done for testing purposes

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

#if NET6_0 || NET7_0

using Swashbuckle.AspNetCore.Annotations;

#endif

namespace Sample.AspNetCore.SwaggerUI.Swashbuckle;

internal static class SwaggerGenConfigurer
{
    internal static void Configure(this SwaggerGenOptions options)
    {
#if NET6_0 || NET7_0
        options.EnableAnnotations();
#endif
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ToDo API",
            Description = "An ASP.NET Core Web API for testing",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "Example License",
                Url = new Uri("https://example.com/license")
            }
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.
                          Enter 'Bearer ' and then your token in the text input below.
                          Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    }
}