+===================================================================================+
|                  NSwag.AspNetCore.Themes • RELEASE NOTES - v3.1                   |
+===================================================================================+

NEW FEATURES
-------------------------------------------------------------------------------------
- Filter Box Support: settings.EnableFilter() is now available for NSwag, enabling
    Swagger UI's native operation filter box
- Pinnable Filter Bar: Pin the operation filter box in place, just like the topbar
    Enable it with: settings.EnableFilter(pinnable: true);


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
  app.UseSwaggerUi(Theme.Dark, settings => settings.EnableFilter(pinnable: true));


DOCUMENTATION
-------------------------------------------------------------------------------------

Full docs:    https://github.com/teociaps/SwaggerUI.Themes/wiki
Repository:   https://github.com/teociaps/SwaggerUI.Themes

=====================================================================================