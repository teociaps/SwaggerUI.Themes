+===================================================================================+
|                AspNetCore.SwaggerUI.Themes • RELEASE NOTES - v3.1                 |
+===================================================================================+

NEW FEATURES
-------------------------------------------------------------------------------------
- Pinnable Filter Bar: Pin the operation filter box in place, just like the topbar
    Enable it with: c.EnableFilter(pinnable: true);


IMPROVEMENTS
-------------------------------------------------------------------------------------
- Pinnable topbar now remembers its pinned/unpinned state across page reloads
- Improved theme asset serving performance
- Fixed a thread-safety issue that could affect parallel test execution


QUICK START
-------------------------------------------------------------------------------------
Pinnable filter bar:
  app.UseSwaggerUI(Theme.Dark, c => c.EnableFilter(pinnable: true));


DOCUMENTATION
-------------------------------------------------------------------------------------

Full docs:       https://github.com/teociaps/SwaggerUI.Themes/wiki
Troubleshooting: https://github.com/teociaps/SwaggerUI.Themes/wiki/Troubleshooting
Repository:      https://github.com/teociaps/SwaggerUI.Themes

=====================================================================================