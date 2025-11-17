# Contributing to SwaggerUI.Themes

Thank you for considering contributing to **SwaggerUI.Themes**! We welcome contributions from the community to help improve and grow the project.

## Code of Conduct

This project and everyone participating in it are governed by our [Code of Conduct](CODE_OF_CONDUCT.md).

## How to Contribute

1. **Fork the repository to your GitHub account.**
2. Clone the forked repository to your local machine.
3. Create a **new branch** for your contribution:

	```bash
	git checkout -b feature/my-feature
	```
	
4. Make your changes and commit them:

	```bash
	git commit -m "Add new feature"
	```

5. Push the changes to your fork:

	```bash
	git push origin feature/my-feature
	```

6. Open a **pull request (PR)** against the **main** branch of the original repository.

## Working with Styles and Scripts

When contributing changes to CSS or JavaScript files for themes, please follow these guidelines to ensure both original and minified versions are properly maintained.

### Minification Process

The project includes both original and minified versions of assets. The minified versions are used at runtime for optimal performance, while original files serve as fallbacks.

#### CSS Files

1. Navigate to [CSS Minifier](https://www.toptal.com/developers/cssminifier)
2. For each modified CSS file (e.g., `common.css`, `dark.css`, etc.):
   - Open the original CSS file and copy its entire content
   - Paste the content into the online minifier
   - Click **"Minify"** to generate the compressed version
   - Copy the minified output
   - **Add the compact header** at the beginning of the minified content (see format below)
   - Paste the result into the corresponding `.min.css` file (e.g., `common.min.css`)
   - **Important**: Verify that placeholders are preserved:
     - For `common.css` and `modern.common.css`, ensure `#STICKY_OPERATIONS` is still present in the minified output

#### JavaScript Files

1. Navigate to [JavaScript Minifier](https://www.toptal.com/developers/javascript-minifier)
2. For each modified JavaScript file (e.g., `modern.js`):
   - Open the original JavaScript file and copy its entire content
   - Paste the content into the online minifier
   - Click **"Minify"** to generate the compressed version
   - Copy the minified output
   - **Add the compact header** at the beginning of the minified content (see format below)
   - Paste the result into the corresponding `.min.js` file (e.g., `modern.min.js`)
   - **Important**: Verify that all placeholders are preserved in the minified output:
     - `{$PINNABLE_TOPBAR}`
     - `{$BACK_TO_TOP}`
     - `{$EXPAND_COLLAPSE_ALL_OPERATIONS}`

#### Minified File Header Format

All minified files must start with a compact comment header for test compatibility. The format is:

```
/*StyleName*/[minified content here...]
```

**Examples**:
- `dark.min.css` → `/*Dark*/body{color:...`
- `modern.dark.min.css` → `/*Modern Dark*/body{color:...`
- `common.min.css` → `/*Common Style*/body{color:...`
- `modern.min.js` → `/*Modern UI*/const rootElement=...`

#### Committing Changes

When committing style or script changes, always include both the original and minified versions:

```bash
git add src/AspNetCore.Swagger.Themes.Common/AspNetCore/Swagger/Themes/Styles/*.css
git add src/AspNetCore.Swagger.Themes.Common/AspNetCore/Swagger/Themes/Scripts/*.js
git commit -m "Update theme styles: [description of changes]"
```

### Why Placeholders Matter

The placeholders (`#STICKY_OPERATIONS`, `{$PINNABLE_TOPBAR}`, etc.) are dynamically replaced at runtime based on user configuration. It is critical that minification preserves these exact strings to maintain functionality.

### Testing Your Changes

Before submitting your pull request:

1. Build the project:
   ```bash
   dotnet build -c Release
   ```

2. Run the tests to ensure minified headers are correct:
   ```bash
   dotnet test
   ```

3. Test with the sample applications:
   ```bash
   dotnet run --project samples/Sample.AspNetCore.SwaggerUI.Swashbuckle
   ```

4. Verify that:
   - Themes render correctly
   - Advanced features work as expected (pinnable topbar, back-to-top button, etc.)
   - No console errors appear in the browser's developer tools
   - Tests pass

## Pull Request Guidelines
- Ensure your code adheres to the existing coding standards.
- Include relevant documentation and test cases for your changes.
- For style/script changes, include both original and minified versions with proper headers.
- Keep your pull request focused. If you are addressing multiple issues, submit separate pull requests for each.
- Be responsive to comments and feedback on your pull request.

## Reporting Bugs and Issues
If you find a bug or have a question, please open an issue on GitHub. When reporting a bug, provide as much detail as possible, including:

- A clear and concise description of the bug.
- Steps to reproduce the issue.
- Expected behavior and actual behavior.
- Screenshots or code snippets, if applicable.

## Feature Requests
We welcome suggestions for new features or improvements. Please open an issue to propose your ideas.

## License
By contributing to this project, you agree that your contributions will be licensed under the [License](LICENSE) file.

Thank you for your contributions!

**_@teociaps_**