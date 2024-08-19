using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Sample.AspNetCore.SwaggerUI.NSwag;

internal static class OpenApiDocGenConfigurer
{
    internal static void Configure(this AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        options.PostProcess = document =>
        {
            document.Info = new OpenApiInfo
            {
                Version = "v1",
                Title = "ToDo API",
                Description = "An ASP.NET Core Web API for managing ToDo items",
                TermsOfService = "https://example.com/terms",
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = "https://example.com/contact"
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = "https://example.com/license"
                }
            };
        };
        options.AddSecurity("bearer", [], new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.OAuth2,
            Description = "My Authentication",
            Flow = OpenApiOAuth2Flow.Implicit,
            Flows = new OpenApiOAuthFlows()
            {
                Implicit = new OpenApiOAuthFlow()
                {
                    Scopes = new Dictionary<string, string>
            {
                { "read", "Read access to protected resources" },
                { "write", "Write access to protected resources" }
            },
                    AuthorizationUrl = "https://localhost:44333/core/connect/authorize",
                    TokenUrl = "https://localhost:44333/core/connect/token"
                },
            }
        });

        options.OperationProcessors.Add(
            new AspNetCoreOperationSecurityScopeProcessor("bearer"));
    }
}