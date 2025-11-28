+===================================================================================+
|                 AspNetCore.SwaggerUI.Themes • RELEASE NOTES - v3                  |
+===================================================================================+

BREAKING CHANGES
-------------------------------------------------------------------------------------
- API redesign: Style -> Theme (all classes renamed)
- Classic themes removed (modern themes only)
- .NET 6 & 7 support discontinued

See migration guide: https://github.com/teociaps/SwaggerUI.Themes/wiki/Migration-v3


NEW FEATURES
-------------------------------------------------------------------------------------
- 🔥 Runtime Theme Switcher: Change themes on-the-fly without refreshing the page
- Auto-Discovery: Custom themes automatically available in the theme switcher
- Nested Folder Support: Organize your themes in subfolders
- Standalone Themes: Create CSS-only themes with zero dependencies
- Smart Filename Resolution: Support standard and standalone variants simultaneously
- Minified Stylesheet Support: Optimize load times with minified CSS


PLATFORM & DEPENDENCIES
-------------------------------------------------------------------------------------
- .NET 10 support added
- Swashbuckle.AspNetCore updated to v9.0.6


IMPROVEMENTS
-------------------------------------------------------------------------------------
- Unified modern theme system with consistent defaults
- Advanced features now available on all themes
- Smaller package footprint (optimized assets)
- Improved performance and clarity throughout


QUICK START
-------------------------------------------------------------------------------------
Basic theme:
  app.UseSwaggerUI(Theme.Dark);

Enable theme switcher:
  app.UseSwaggerUI(Theme.Dark, c => c.EnableThemeSwitcher());

All features:
  app.UseSwaggerUI(Theme.Dark, c => c.EnableAllAdvancedOptions());


DOCUMENTATION
-------------------------------------------------------------------------------------

Full docs:    https://github.com/teociaps/SwaggerUI.Themes/wiki
Migration:    https://github.com/teociaps/SwaggerUI.Themes/wiki/Migration-v3
Repository:   https://github.com/teociaps/SwaggerUI.Themes

=====================================================================================