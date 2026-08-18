# Contributing to SwaggerUI.Themes

Thank you for considering contributing to **SwaggerUI.Themes**! We welcome contributions from the community to help improve and grow the project.

## Code of Conduct

This project and everyone participating in it are governed by our [Code of Conduct](CODE_OF_CONDUCT.md).

## How to Contribute

1. **Fork the repository** to your GitHub account.
2. **Clone** the forked repository to your local machine.
3. Create a **new branch** for your contribution:

	```bash
	git checkout -b feature/my-feature
	```
	
4. Make your changes and **commit** them:

	```bash
	git commit -m "Add new feature"
	```

5. **Push** the changes to your fork:

	```bash
	git push origin feature/my-feature
	```

6. Open a **pull request (PR)** against the **main** branch of the original repository.

## Project Structure

Understanding the project layout helps you navigate the codebase:

- **`src/AspNetCore.Swagger.Themes.Common/`** - Core theme system shared between packages
- **`src/AspNetCore.SwaggerUI.Themes/`** - Swashbuckle.AspNetCore theme integration
- **`src/NSwag.AspNetCore.Themes/`** - NSwag.AspNetCore theme integration
- **`src/AspNetCore.SwaggerUI.Extensions/`** - Swashbuckle.AspNetCore functional extensions
- **`src/NSwag.AspNetCore.Extensions/`** - NSwag.AspNetCore functional extensions
- **`samples/`** - Example applications demonstrating usage
- **`tests/`** - Unit and integration tests

---

## Contributing Themes

Themes customize the visual appearance of Swagger UI. When contributing a new theme:

### Adding or Modifying Themes

Themes are defined as CSS files with optional JavaScript for advanced features. When working with themes:

1. **Original files** - Make your changes in the non-minified `.css` or `.js` files
2. **Minified versions** - Must be manually updated (see Minification Process below)
3. **Headers** - Minified files require specific comment headers for testing

### Minification Process

The project uses minified versions at runtime for performance. After editing CSS or JavaScript:

#### For CSS Files

1. Navigate to [CSS Minifier](https://www.toptal.com/developers/cssminifier)
2. Copy the **entire content** of your modified CSS file
3. Paste into the minifier and click **"Minify"**
4. Copy the minified output
5. Add the **compact header** at the very beginning (format below)
6. Paste into the corresponding `.min.css` file
7. **Verify placeholders** are preserved (e.g., `#STICKY_OPERATIONS` in `common.css`)

#### For JavaScript Files

1. Navigate to [JavaScript Minifier](https://www.toptal.com/developers/javascript-minifier)
2. Copy the **entire content** of your modified JavaScript file
3. Paste into the minifier and click **"Minify"**
4. Copy the minified output
5. Add the **compact header** at the very beginning (format below)
6. Paste into the corresponding `.min.js` file
7. **Verify all placeholders** are preserved:
   - `{$PINNABLE_TOPBAR}`
   - `{$BACK_TO_TOP}`
   - `{$EXPAND_COLLAPSE_ALL_OPERATIONS}`
   - `{$THEME_SWITCHER}`

#### Minified Header Format

All minified files must start with this compact comment:

```
/*Name*/[minified content]
```

**Examples:**
- `dark.min.css` → `/*Dark Theme*/body{...`
- `common.min.css` → `/*Common*/body{...`
- `ui.min.js` → `/*Swagger UI*/const ...`

> **Why?** Tests verify headers are present and correctly formatted. This ensures minification was done properly.

#### Placeholder Preservation

Placeholders like `{$THEME_SWITCHER}` are replaced at runtime based on user settings. Minification **must preserve** these exact strings, or features will break.

### Committing Theme Changes

Always commit both original **and** minified versions:

```bash
git add src/AspNetCore.Swagger.Themes.Common/AspNetCore/Swagger/Themes/Styles/*.css
git add src/AspNetCore.Swagger.Themes.Common/AspNetCore/Swagger/Themes/Scripts/*.js
git commit -m "Add new ocean theme"
```

### Theme Organization

When adding example custom themes, use subfolders for clarity:

```
samples/YourSample/SwaggerThemes/
├── CompanyThemes/
│   └── corporate-blue.css
└── SeasonalThemes/
    └── holiday-red.css
```

This demonstrates best practices for users organizing their own themes.

### Standalone vs. Regular Themes

- **Regular themes** - Depend on `common.css` and may use JavaScript features
- **Standalone themes** - Prefix with `standalone.` (e.g., `standalone.custom.css`) for zero dependencies

Choose based on whether you need shared styles or prefer total independence.

---

## Contributing Extensions

Extensions add functional capabilities to Swagger UI beyond theming. When contributing a new extension:

### Planning an Extension

Before implementing, consider:

1. **Scope** - Is it a focused feature or part of a larger system?
2. **Compatibility** - Does it work with both Swashbuckle and NSwag?
3. **Integration points** - Where should it hook into the pipeline?
4. **User value** - What problem does it solve for users?

### Implementation Guidelines

Extensions should be implemented for both integrations:

- **Swashbuckle implementation** - `src/AspNetCore.SwaggerUI.Extensions/`
  - Add service extension methods in `Microsoft.Extensions.DependencyInjection/`
  - Add filters in `AspNetCore/Swagger/Extensions/Filters/`

- **NSwag implementation** - `src/NSwag.AspNetCore.Extensions/`
  - Add service extension methods in `Microsoft.Extensions.DependencyInjection/`
  - Add processors in `AspNetCore/Swagger/Extensions/Processors/`

### Extension Structure Example

```
src/AspNetCore.SwaggerUI.Extensions/
├── Microsoft/Extensions/DependencyInjection/
│   └── SwaggerGenOptionsExtensions.cs      // Service configuration
├── AspNetCore/Swagger/Extensions/
│   └── Filters/
│       └── MyFeatureFilter.cs              // Swagger/OpenAPI filter
└── package-readme.md                       // NuGet package documentation

src/NSwag.AspNetCore.Extensions/
├── Microsoft/Extensions/DependencyInjection/
│   └── AspNetCoreOpenApiDocumentGeneratorSettingsExtensions.cs
├── AspNetCore/Swagger/Extensions/
│   └── Processors/
│       └── MyFeatureProcessor.cs           // NSwag processor
└── package-readme.md
```

### Testing Extensions

1. Add tests in the appropriate test project
2. Test with both sample applications
3. Verify the extension works independently and alongside other extensions

---

## Testing Your Changes

Before submitting a pull request:

### 1. Build the Solution

```bash
dotnet build -c Release
```

Ensure there are no compilation errors.

### 2. Run Tests

```bash
dotnet test
```

All tests must pass, including:
- Minified header format tests (for themes)
- Theme discovery tests (for themes)
- Extension functionality tests (for extensions)
- Integration tests

### 3. Test with Sample Applications

Run both sample projects to verify functionality:

```bash
# Swashbuckle sample
dotnet run --project samples/Sample.AspNetCore.SwaggerUI.Swashbuckle

# NSwag sample
dotnet run --project samples/Sample.AspNetCore.SwaggerUI.NSwag

# Swashbuckle Extensions sample
dotnet run --project samples/Sample.AspNetCore.SwaggerUI.Extensions

# NSwag Extensions sample
dotnet run --project samples/Sample.NSwag.AspNetCore.Extensions
```

### 4. Manual Verification

Open the sample app in your browser and verify:

- ✅ Themes render correctly (if theme changes)
- ✅ Theme switcher dropdown appears and works (if theme changes)
- ✅ Custom themes are discovered automatically (if theme changes)
- ✅ Extensions work as expected (if extension changes)
- ✅ Advanced features work (pinnable topbar, back-to-top, etc.)
- ✅ No console errors in browser developer tools
- ✅ Standalone themes load without extra dependencies

## Pull Request Guidelines

- **Follow existing code style** - Match the patterns you see in the codebase
- **Include tests** - Add or update tests for your changes
- **Update documentation** - If adding features, update relevant docs
- **Keep PRs focused** - One feature or fix per PR (submit separate PRs for multiple changes)
- **Provide context** - Explain *why* the change is needed, not just *what* changed
- **Be responsive** - Address feedback and questions promptly

### Good PR Practices

- Use descriptive titles: `feat: add sunset theme` or `feat: add api metadata extension`
- Reference issues: `Fixes #123` or `Closes #456`
- Include screenshots for visual changes (themes)
- Include before/after examples for functional changes (extensions)
- Test across .NET 8, 9, and 10 if possible

## Reporting Bugs

Found a bug? Please open an issue with:

- **Clear description** of the problem
- **Steps to reproduce** the issue
- **Expected vs. actual behavior**
- **Environment details:**
  - Which package (Themes or Extensions)
  - Which integration (Swashbuckle or NSwag)
  - .NET version
  - Browser (if UI-related)
- **Screenshots or code snippets** if applicable

See the bug report template for a structured approach.

## Suggesting Features

We welcome feature suggestions! When proposing a feature:

- **Describe the use case** - What problem does it solve?
- **Specify the scope** - Is it for Themes, Extensions, or both?
- **Explain the benefit** - How does it improve the project?
- **Consider backward compatibility** - Will it break existing code?
- **Offer to help** - Can you contribute the implementation?

See the feature request template for a structured approach.

## Development Tips

### Quick Testing Loop

For rapid iteration during development:

1. Make your changes
2. Build: `dotnet build`
3. Run sample: `dotnet run --project samples/Sample.AspNetCore.SwaggerUI.Swashbuckle`
4. Refresh browser (F5) (clear the browser cache if necessary) to see changes

### Organizing Custom Themes

When adding example custom themes, use subfolders for clarity:

```
samples/YourSample/SwaggerThemes/
├── CompanyThemes/
│   └── corporate-blue.css
└── SeasonalThemes/
    └── holiday-red.css
```

This demonstrates best practices for users organizing their own themes.

## Questions?

Not sure about something? Feel free to:

- Open an issue for clarification
- Ask in your pull request
- Reference existing code patterns

## License

By contributing, you agree that your contributions will be licensed under the same [MIT License](LICENSE) that covers the project.

---

Thank you for making SwaggerUI.Themes better! 🎨

**[@teociaps](https://github.com/teociaps)**