using Sample.AspNetCore.SwaggerUI.NSwag;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(OpenApiDocGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();

    // ========================================
    // 🎨 BASIC USAGE EXAMPLES
    // ========================================

    // 1. Simple predefined theme (no theme switcher)
    //app.UseSwaggerUi(Theme.Dark, c =>
    //{
    //    c.DocumentTitle = "Sample API - Dark Theme";
    //});

    // 2. Inline CSS theme
    //app.UseSwaggerUi("body { background-color: #1a1a2e; color: #eee; }", c =>
    //{
    //    c.DocumentTitle = "Sample API - Inline Custom Style";
    //});

    // 3. Custom theme from embedded resource
    app.UseSwaggerUi(Assembly.GetExecutingAssembly(), "mybrand.css", c =>
    {
        c.DocumentTitle = "Sample API - Custom Theme from Assembly";
        c.EnableAllAdvancedOptions(); // Enables theme switcher + all UI features
    });

    // 4. Standalone theme (no dependencies on common.css or ui.js)
    //app.UseSwaggerUi(Assembly.GetExecutingAssembly(), "standalone.custom.css", c =>
    //{
    //    c.DocumentTitle = "Sample API - Standalone Theme";
    //    // Note: Standalone themes don't support advanced options
    //});

    // ========================================
    // 🔀 THEME SWITCHER EXAMPLES
    // ========================================

    // 5. All themes (predefined + custom auto-discovery) - DEFAULT
    //app.UseSwaggerUi(Theme.Dark, c =>
    //{
    //    c.DocumentTitle = "Sample API - All Themes";
    //    c.EnableThemeSwitcher(); // Shows all 6 predefined + discovered custom themes
    //});

    // 6. Predefined themes only
    //app.UseSwaggerUi(Theme.Forest, c =>
    //{
    //    c.DocumentTitle = "Sample API - Predefined Only";
    //    c.EnableThemeSwitcher(ThemeSwitcherOptions.PredefinedOnly());
    //});

    // 7. Custom themes only (auto-discovery)
    //app.UseSwaggerUi(CustomTheme.Custom, c =>
    //{
    //    c.DocumentTitle = "Sample API - Custom Themes Only";
    //    c.EnableThemeSwitcher(ThemeSwitcherOptions.CustomOnly());
    //});

    // 8. Specific themes selection (mixed predefined + custom)
    //app.UseSwaggerUi(CompanyThemes.CorporateBlue, c =>
    //{
    //    c.DocumentTitle = "Sample API - Business Themes";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            Theme.Dark,
    //            Theme.Light,
    //            CompanyThemes.CorporateBlue,
    //            CompanyThemes.TechGreen,
    //            CompanyThemes.StartupPurple
    //        ));
    //});

    // 9. Exclude specific themes
    //app.UseSwaggerUi(Theme.DeepSea, c =>
    //{
    //    c.DocumentTitle = "Sample API - Excluding Themes";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .ExcludeThemes(Theme.Futuristic, Theme.Desert));
    //});

    // 10. Custom display format
    //app.UseSwaggerUi(Theme.Desert, c =>
    //{
    //    c.DocumentTitle = "Sample API - Custom Display Format";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithDisplayFormat("🎨 {name}"));
    //});

    // 11. Seasonal/Holiday themes
    //app.UseSwaggerUi(SeasonalThemes.HolidayRed, c =>
    //{
    //    c.DocumentTitle = "Sample API - Holiday Special 🎄";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            SeasonalThemes.HolidayRed,
    //            SeasonalThemes.OceanBlue,
    //            Theme.Dark,
    //            Theme.Light
    //        ));
    //});

    // 12. Professional company branding
    //app.UseSwaggerUi(CompanyThemes.TechGreen, c =>
    //{
    //    c.DocumentTitle = "Company API Documentation";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            CompanyThemes.CorporateBlue,
    //            CompanyThemes.TechGreen,
    //            CompanyThemes.StartupPurple
    //        )
    //        .WithDisplayFormat("Company Theme: {name}"));
    //});

    // ========================================
    // ⚙️ ADVANCED UI FEATURES (without theme switcher)
    // ========================================

    // 13. Enable all advanced UI features
    //app.UseSwaggerUi(Theme.Dark, c =>
    //{
    //    c.DocumentTitle = "Sample API - All Features";
    //    c.EnableAllAdvancedOptions(); // Pinnable topbar, back-to-top, sticky ops, expand/collapse, theme switcher
    //});

    // 14. Individual advanced features
    //app.UseSwaggerUi(Theme.Light, c =>
    //{
    //    c.DocumentTitle = "Sample API - Selected Features";
    //    c.EnablePinnableTopbar();
    //    c.ShowBackToTopButton();
    //    c.EnableStickyOperations();
    //    c.EnableExpandOrCollapseAllOperations();
    //    // Note: Not enabling theme switcher here
    //});

    // 15. Advanced features with custom theme
    //app.UseSwaggerUi(CustomTheme.Custom, c =>
    //{
    //    c.DocumentTitle = "Sample API - Custom with Features";
    //    c.EnablePinnableTopbar();
    //    c.ShowBackToTopButton();
    //    c.EnableThemeSwitcher();
    //});

    // ========================================
    // 🎯 ADVANCED THEME SWITCHER OPTIONS
    // ========================================

    // 16. ExplicitOnly mode (no auto-discovery)
    //app.UseSwaggerUi(CompanyThemes.CorporateBlue, c =>
    //{
    //    c.DocumentTitle = "Sample API - Explicit Themes";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            Theme.Dark,
    //            Theme.Light,
    //            CompanyThemes.CorporateBlue
    //        )
    //        .WithCustomThemes(CustomThemeMode.ExplicitOnly));
    //});

    // 17. Complex filtering
    //app.UseSwaggerUi(Theme.Forest, c =>
    //{
    //    c.DocumentTitle = "Sample API - Complex Filtering";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithAllPredefinedThemes(true) // Include all 6 predefined
    //        .ExcludeThemes(Theme.Futuristic) // But exclude Futuristic
    //        .WithCustomThemes(CustomThemeMode.AutoDiscover)); // Auto-discover custom
    //});

    // 18. Minimal configuration (just 2 themes)
    //app.UseSwaggerUi(Theme.Dark, c =>
    //{
    //    c.DocumentTitle = "Sample API - Minimal";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(Theme.Dark, Theme.Light)
    //        .WithCustomThemes(CustomThemeMode.None));
    //});
}

app.UseHttpsRedirection();

app.AddEndpoints();
app.MapControllers();

await app.RunAsync();