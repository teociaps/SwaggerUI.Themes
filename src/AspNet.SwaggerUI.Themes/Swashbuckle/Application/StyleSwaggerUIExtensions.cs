using AspNet.SwaggerUI.Themes;
using System;
using System.Reflection;

namespace Swashbuckle.Application
{
    /// <summary>
    /// Extensions methods for <see cref="SwaggerEnabledConfiguration"/>.
    /// </summary>
    public static class StyleSwaggerUIExtensions
    {
        private static readonly Assembly _thisAssembly = typeof(StyleSwaggerUIExtensions).Assembly;

        /// <summary>
        /// Enables Swagger UI with a specified style.
        /// </summary>
        /// <param name="configuration">The Swagger enabled configuration.</param>
        /// <param name="style">The style to apply to Swagger UI.</param>
        /// <param name="configure">Optional configuration action for Swagger UI.</param>
        public static void EnableSwaggerUi(this SwaggerEnabledConfiguration configuration, Style style, Action<SwaggerUiConfig> configure = null)
        {
            var defaultSetupAction = ConfigureStyle(style, configure);

            configuration.EnableSwaggerUi(defaultSetupAction);
        }

        /// <summary>
        /// Enables Swagger UI with a specified style and custom route template.
        /// </summary>
        /// <param name="configuration">The Swagger enabled configuration.</param>
        /// <param name="routeTemplate">The custom route template for Swagger UI.</param>
        /// <param name="style">The style to apply to Swagger UI.</param>
        /// <param name="configure">Optional configuration action for Swagger UI.</param>
        public static void EnableSwaggerUi(this SwaggerEnabledConfiguration configuration, string routeTemplate, Style style, Action<SwaggerUiConfig> configure = null)
        {
            var defaultSetupAction = ConfigureStyle(style, configure);

            configuration.EnableSwaggerUi(routeTemplate, defaultSetupAction);
        }

        private static Action<SwaggerUiConfig> InjectStyle(Style style)
        {
            return c => c.InjectStylesheet(_thisAssembly, $"{_thisAssembly.GetName().Name}.Styles.{style.FileName}");
        }

        private static Action<SwaggerUiConfig> ConfigureStyle(Style style, Action<SwaggerUiConfig> configure)
        {
            var defaultSetupAction = InjectStyle(style);
            if (configure != null)
            {
                defaultSetupAction += configure;
            }

            return defaultSetupAction;
        }
    }
}