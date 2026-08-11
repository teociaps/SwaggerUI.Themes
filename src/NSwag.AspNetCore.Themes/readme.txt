+===================================================================================+
|                  NSwag.AspNetCore.Themes • RELEASE NOTES - v3.1                   |
+===================================================================================+

NEW FEATURES
-------------------------------------------------------------------------------------
- Filter Box Support: settings.EnableFilter() is now available for NSwag, enabling
    Swagger UI's native operation filter box (previously only available through the
    Swashbuckle package)
- Pinnable Filter Bar: Pin the operation filter box in place, just like the topbar
    Enable it with: settings.EnableFilter(pinned: true);


IMPROVEMENTS
-------------------------------------------------------------------------------------
- Pinnable topbar now remembers its pinned/unpinned state across page reloads
- Improved theme asset serving performance
- Fixed a thread-safety issue that could affect parallel test execution


QUICK START
-------------------------------------------------------------------------------------
Filter box:
  app.UseSwaggerUi(Theme.Dark, settings => settings.EnableFilter());

Pinnable filter bar:
  app.UseSwaggerUi(Theme.Dark, settings => settings.EnableFilter(pinned: true));


DOCUMENTATION
-------------------------------------------------------------------------------------

Full docs:    https://github.com/teociaps/SwaggerUI.Themes/wiki
Repository:   https://github.com/teociaps/SwaggerUI.Themes

=====================================================================================


+===================================================================================+
|                   NSwag.AspNetCore.Themes • RELEASE NOTES - v3                    |
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
- NSwag.AspNetCore updated to v14.6.3


IMPROVEMENTS
-------------------------------------------------------------------------------------
- Unified modern theme system with consistent defaults
- Advanced features now available on all themes
- Smaller package footprint (optimized assets)
- Improved performance and clarity throughout


QUICK START
-------------------------------------------------------------------------------------
Basic theme:
  app.UseSwaggerUi(Theme.Dark);

Enable theme switcher:
  app.UseSwaggerUi(Theme.Dark, c => c.EnableThemeSwitcher());

All features:
  app.UseSwaggerUi(Theme.Dark, c => c.EnableAllAdvancedOptions());


DOCUMENTATION
-------------------------------------------------------------------------------------

Full docs:    https://github.com/teociaps/SwaggerUI.Themes/wiki
Migration:    https://github.com/teociaps/SwaggerUI.Themes/wiki/Migration-v3
Repository:   https://github.com/teociaps/SwaggerUI.Themes

=====================================================================================