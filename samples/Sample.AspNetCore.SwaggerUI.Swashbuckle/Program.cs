using AspNetCore.Swagger.Themes;
using Sample.AspNetCore.SwaggerUI.Swashbuckle;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerGenConfigurer.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // ========================================
    // 🎨 BASIC USAGE EXAMPLES
    // ========================================

    // 1. Simple predefined theme (no theme switcher)
    app.UseSwaggerUI(Theme.Dark, c => c.DocumentTitle = "Sample API - Dark Theme");

    // 2. Inline CSS theme
    //app.UseSwaggerUI("body { background-color: #1a1a2e; color: #eee; }", c =>
    //{
    //    c.DocumentTitle = "Sample API - Inline Custom Style";
    //});

    // 3. Custom theme from embedded resource
    //app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "mybrand.css", c =>
    //{
    //    c.DocumentTitle = "Sample API - Custom Theme from Assembly";
    //    c.EnableAllAdvancedOptions(); // Enables theme switcher + all UI features
    //});

    // 4. Standalone theme (no dependencies on common.css or ui.js)
    //app.UseSwaggerUI(Assembly.GetExecutingAssembly(), "standalone.custom.css", c =>
    //{
    //    c.DocumentTitle = "Sample API - Standalone Theme";
    //    // Note: Standalone themes don't support advanced options
    //});

    // ========================================
    // 🔀 THEME SWITCHER EXAMPLES
    // ========================================

    // 5. All themes (predefined + custom auto-discovery) - DEFAULT
    //app.UseSwaggerUI(Theme.Dark, c =>
    //{
    //    c.DocumentTitle = "Sample API - All Themes";
    //    c.EnableThemeSwitcher(); // Shows all 6 predefined + discovered custom themes
    //});

    // 6. Predefined themes only
    //app.UseSwaggerUI(Theme.Forest, c =>
    //{
    //    c.DocumentTitle = "Sample API - Predefined Only";
    //    c.EnableThemeSwitcher(ThemeSwitcherOptions.PredefinedOnly());
    //});

    // 7. Custom themes only (auto-discovery)
    //app.UseSwaggerUI(CustomTheme.Custom, c =>
    //{
    //    c.DocumentTitle = "Sample API - Custom Themes Only";
    //    c.EnableThemeSwitcher(ThemeSwitcherOptions.CustomOnly());
    //});

    // 8. Specific themes selection (mixed predefined + custom)
    //app.UseSwaggerUI(CompanyThemes.CompanyThemes.CorporateBlue, c =>
    //{
    //    c.DocumentTitle = "Sample API - Business Themes";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            Theme.Dark,
    //            Theme.Light,
    //            CompanyThemes.CompanyThemes.CorporateBlue,
    //            CompanyThemes.CompanyThemes.TechGreen,
    //            CompanyThemes.CompanyThemes.StartupPurple
    //        ));
    //});

    // 9. Exclude specific themes
    //app.UseSwaggerUI(Theme.DeepSea, c =>
    //{
    //    c.DocumentTitle = "Sample API - Excluding Themes";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .ExcludeThemes(Theme.Futuristic, Theme.Desert));
    //});

    // 10. Custom display format
    //app.UseSwaggerUI(Theme.Desert, c =>
    //{
    //    c.DocumentTitle = "Sample API - Custom Display Format";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithDisplayFormat("🎨 {name}"));
    //});

    // 11. Seasonal/Holiday themes
    //app.UseSwaggerUI(SeasonalThemes.HolidayRed, c =>
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
    //app.UseSwaggerUI(CompanyThemes.CompanyThemes.TechGreen, c =>
    //{
    //    c.DocumentTitle = "Company API Documentation";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            CompanyThemes.CompanyThemes.CorporateBlue,
    //            CompanyThemes.CompanyThemes.TechGreen,
    //            CompanyThemes.CompanyThemes.StartupPurple
    //        )
    //        .WithDisplayFormat("Company Theme: {name}"));
    //});

    // ========================================
    // ⚙️ ADVANCED UI FEATURES
    // ========================================

    // 13. Enable all advanced UI features
    //app.UseSwaggerUI(Theme.Dark, c =>
    //{
    //    c.DocumentTitle = "Sample API - All Features";
    //    c.EnableAllAdvancedOptions(); // Pinnable topbar, back-to-top, sticky ops, expand/collapse, theme switcher
    //});

    // 14. Individual advanced features
    //app.UseSwaggerUI(Theme.Light, c =>
    //{
    //    c.DocumentTitle = "Sample API - Selected Features";
    //    c.EnablePinnableTopbar();
    //    c.ShowBackToTopButton();
    //    c.EnableStickyOperations();
    //    c.EnableExpandOrCollapseAllOperations();
    //    // Note: Not enabling theme switcher here
    //});

    // 15. Advanced features with custom theme
    //app.UseSwaggerUI(CustomTheme.Custom, c =>
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
    //app.UseSwaggerUI(CompanyThemes.CompanyThemes.CorporateBlue, c =>
    //{
    //    c.DocumentTitle = "Sample API - Explicit Themes";
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithThemes(
    //            Theme.Dark,
    //            Theme.Light,
    //            CompanyThemes.CompanyThemes.CorporateBlue
    //        )
    //        .WithCustomThemes(CustomThemeMode.ExplicitOnly));
    //});

    // 17. Complex filtering
    //app.UseSwaggerUI(Theme.Forest, c =>
    //{
    //    c.DocumentTitle = "Sample API - Complex Filtering";
    //    c.EnableAllAdvancedOptions();
    //    c.EnableThemeSwitcher(new ThemeSwitcherOptions()
    //        .WithAllPredefinedThemes(true) // Include all 6 predefined
    //        .ExcludeThemes(Theme.Futuristic) // But exclude Futuristic
    //        .WithCustomThemes(CustomThemeMode.AutoDiscover)); // Auto-discover custom
    //});

    // 18. Minimal configuration (just 2 themes)
    //app.UseSwaggerUI(Theme.Dark, c =>
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