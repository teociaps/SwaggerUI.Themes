using Shouldly;

namespace AspNetCore.Swagger.Themes.Tests;

/// <summary>
/// Comprehensive tests for the Theme Switcher functionality.
/// </summary>
public class ThemeSwitcherTests
{
    #region ThemeSwitcherOptions Tests

    [Fact]
    public void ThemeSwitcherOptions_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var options = new ThemeSwitcherOptions();

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeTrue();
        options.CustomThemeMode.ShouldBe(CustomThemeMode.AutoDiscover);
        options.ThemeDisplayFormat.ShouldBe("{name}");
        options.IncludedThemes.ShouldBeEmpty();
        options.ExcludedThemes.ShouldBeEmpty();
    }

    [Fact]
    public void ThemeSwitcherOptions_All_ReturnsDefaultConfiguration()
    {
        // Arrange & Act
        var options = ThemeSwitcherOptions.All();

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeTrue();
        options.CustomThemeMode.ShouldBe(CustomThemeMode.AutoDiscover);
    }

    [Fact]
    public void ThemeSwitcherOptions_PredefinedOnly_ExcludesCustomThemes()
    {
        // Arrange & Act
        var options = ThemeSwitcherOptions.PredefinedOnly();

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeTrue();
        options.CustomThemeMode.ShouldBe(CustomThemeMode.None);
    }

    [Fact]
    public void ThemeSwitcherOptions_CustomOnly_ExcludesPredefinedThemes()
    {
        // Arrange & Act
        var options = ThemeSwitcherOptions.CustomOnly();

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeFalse();
        options.CustomThemeMode.ShouldBe(CustomThemeMode.AutoDiscover);
    }

    #endregion ThemeSwitcherOptions Tests

    #region CustomThemeMode Tests

    [Fact]
    public void CustomThemeMode_None_ExcludesAllCustomThemes()
    {
        // Arrange
        var options = new ThemeSwitcherOptions
        {
            CustomThemeMode = CustomThemeMode.None
        };

        // Assert
        options.CustomThemeMode.ShouldBe(CustomThemeMode.None);
    }

    [Fact]
    public void CustomThemeMode_ExplicitOnly_IncludesOnlyRegisteredCustomThemes()
    {
        // Arrange
        var options = new ThemeSwitcherOptions
        {
            CustomThemeMode = CustomThemeMode.ExplicitOnly
        };

        // Assert
        options.CustomThemeMode.ShouldBe(CustomThemeMode.ExplicitOnly);
    }

    [Fact]
    public void CustomThemeMode_AutoDiscover_EnablesAutoDiscovery()
    {
        // Arrange
        var options = new ThemeSwitcherOptions
        {
            CustomThemeMode = CustomThemeMode.AutoDiscover
        };

        // Assert
        options.CustomThemeMode.ShouldBe(CustomThemeMode.AutoDiscover);
    }

    #endregion CustomThemeMode Tests

    #region Fluent API Tests

    [Fact]
    public void WithThemes_SetsIncludedThemesAndDisablesIncludeAll()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;

        // Act
        options.WithThemes(darkTheme, lightTheme);

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeFalse();
        options.IncludedThemes.Count.ShouldBe(2);
        options.IncludedThemes.ShouldContain(darkTheme);
        options.IncludedThemes.ShouldContain(lightTheme);
    }

    [Fact]
    public void WithThemes_ClearsPreviousIncludedThemes()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;
        var forestTheme = Theme.Forest;

        options.WithThemes(darkTheme);

        // Act
        options.WithThemes(lightTheme, forestTheme);

        // Assert
        options.IncludedThemes.Count.ShouldBe(2);
        options.IncludedThemes.ShouldNotContain(darkTheme);
        options.IncludedThemes.ShouldContain(lightTheme);
        options.IncludedThemes.ShouldContain(forestTheme);
    }

    [Fact]
    public void ExcludeThemes_SetsExcludedThemes()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();
        var futuristicTheme = Theme.Futuristic;
        var desertTheme = Theme.Desert;

        // Act
        options.ExcludeThemes(futuristicTheme, desertTheme);

        // Assert
        options.ExcludedThemes.Count.ShouldBe(2);
        options.ExcludedThemes.ShouldContain(futuristicTheme);
        options.ExcludedThemes.ShouldContain(desertTheme);
    }

    [Fact]
    public void ExcludeThemes_ClearsPreviousExclusions()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;

        options.ExcludeThemes(darkTheme);

        // Act
        options.ExcludeThemes(lightTheme);

        // Assert
        options.ExcludedThemes.Count.ShouldBe(1);
        options.ExcludedThemes.ShouldNotContain(darkTheme);
        options.ExcludedThemes.ShouldContain(lightTheme);
    }

    [Fact]
    public void WithDisplayFormat_SetsCustomFormat()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();

        // Act
        options.WithDisplayFormat("Theme: {name}");

        // Assert
        options.ThemeDisplayFormat.ShouldBe("Theme: {name}");
    }

    [Fact]
    public void WithDisplayFormat_NullSetsDefaultFormat()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();
        options.WithDisplayFormat("Custom");

        // Act
        options.WithDisplayFormat(null);

        // Assert
        options.ThemeDisplayFormat.ShouldBe("{name}");
    }

    [Fact]
    public void WithAllPredefinedThemes_SetsFlag()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();

        // Act
        options.WithAllPredefinedThemes(false);

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeFalse();
    }

    [Fact]
    public void WithCustomThemes_SetsMode()
    {
        // Arrange
        var options = new ThemeSwitcherOptions();

        // Act
        options.WithCustomThemes(CustomThemeMode.ExplicitOnly);

        // Assert
        options.CustomThemeMode.ShouldBe(CustomThemeMode.ExplicitOnly);
    }

    [Fact]
    public void FluentAPI_CanChainMultipleMethods()
    {
        // Arrange
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;
        var futuristicTheme = Theme.Futuristic;

        // Act
        var options = new ThemeSwitcherOptions()
            .WithThemes(darkTheme, lightTheme)
            .ExcludeThemes(futuristicTheme)
            .WithDisplayFormat("🎨 {name}")
            .WithCustomThemes(CustomThemeMode.AutoDiscover);

        // Assert
        options.IncludeAllPredefinedThemes.ShouldBeFalse();
        options.IncludedThemes.Count.ShouldBe(2);
        options.ExcludedThemes.Count.ShouldBe(1);
        options.ThemeDisplayFormat.ShouldBe("🎨 {name}");
        options.CustomThemeMode.ShouldBe(CustomThemeMode.AutoDiscover);
    }

    #endregion Fluent API Tests

    #region Validation Tests

    [Fact]
    public void Validate_ThrowsWhenLessThanTwoThemesAvailable()
    {
        // Arrange
        var darkTheme = Theme.Dark;
        var options = new ThemeSwitcherOptions()
            .WithThemes(darkTheme)
            .WithCustomThemes(CustomThemeMode.None);

        // Act & Assert
        var exception = Should.Throw<InvalidOperationException>(() => options.Validate("Dark"));
        exception.Message.ShouldContain("at least 2 themes");
    }

    [Fact]
    public void Validate_PassesWithTwoOrMoreThemes()
    {
        // Arrange
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;
        var options = new ThemeSwitcherOptions()
            .WithThemes(darkTheme, lightTheme)
            .WithCustomThemes(CustomThemeMode.None);

        // Act & Assert
        Should.NotThrow(() => options.Validate("Dark"));
    }

    [Fact]
    public void Validate_ThrowsWhenDefaultThemeExcluded()
    {
        // Arrange
        var darkTheme = Theme.Dark;
        var options = new ThemeSwitcherOptions()
            .ExcludeThemes(darkTheme);

        // Act & Assert
        var exception = Should.Throw<InvalidOperationException>(() => options.Validate("Dark"));
        exception.Message.ShouldContain("cannot be excluded");
    }

    [Fact]
    public void Validate_ThrowsWhenDefaultThemeNotIncludedInSpecificList()
    {
        // Arrange
        var lightTheme = Theme.Light;
        var forestTheme = Theme.Forest;
        var options = new ThemeSwitcherOptions()
            .WithThemes(lightTheme, forestTheme)
            .WithCustomThemes(CustomThemeMode.None);

        // Act & Assert
        var exception = Should.Throw<InvalidOperationException>(() => options.Validate("Dark"));
        exception.Message.ShouldContain("must be included");
    }

    [Fact]
    public void Validate_PassesWhenDefaultThemeIncluded()
    {
        // Arrange
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;
        var options = new ThemeSwitcherOptions()
            .WithThemes(darkTheme, lightTheme);

        // Act & Assert
        Should.NotThrow(() => options.Validate("Dark"));
    }

    #endregion Validation Tests

    #region FileProvider Tests

    [Fact]
    public void FileProvider_IsPredefinedTheme_ReturnsTrueForPredefinedThemes()
    {
        // Arrange & Act & Assert
        FileProvider.IsPredefinedTheme("Dark").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("Light").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("Forest").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("DeepSea").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("Desert").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("Futuristic").ShouldBeTrue();
    }

    [Fact]
    public void FileProvider_IsPredefinedTheme_IsCaseInsensitive()
    {
        // Arrange & Act & Assert
        FileProvider.IsPredefinedTheme("dark").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("LIGHT").ShouldBeTrue();
        FileProvider.IsPredefinedTheme("FoReSt").ShouldBeTrue();
    }

    [Fact]
    public void FileProvider_IsPredefinedTheme_ReturnsFalseForCustomThemes()
    {
        // Arrange & Act & Assert
        FileProvider.IsPredefinedTheme("Custom").ShouldBeFalse();
        FileProvider.IsPredefinedTheme("MyTheme").ShouldBeFalse();
        FileProvider.IsPredefinedTheme("").ShouldBeFalse();
        FileProvider.IsPredefinedTheme("NotATheme").ShouldBeFalse();
    }

    #endregion FileProvider Tests

    #region Theme Name Tests

    [Fact]
    public void Theme_GetThemeName_ReturnsCorrectNames()
    {
        // Arrange & Act & Assert
        Theme.Dark.GetThemeName().ShouldBe("Dark");
        Theme.Light.GetThemeName().ShouldBe("Light");
        Theme.Forest.GetThemeName().ShouldBe("Forest");
        Theme.DeepSea.GetThemeName().ShouldBe("Deepsea");
        Theme.Desert.GetThemeName().ShouldBe("Desert");
        Theme.Futuristic.GetThemeName().ShouldBe("Futuristic");
    }

    [Fact]
    public void Theme_FileName_HasCorrectExtension()
    {
        // Arrange & Act & Assert
        Theme.Dark.FileName.ShouldEndWith(".min.css");
        Theme.Light.FileName.ShouldEndWith(".min.css");
        Theme.Forest.FileName.ShouldEndWith(".min.css");
    }

    #endregion Theme Name Tests

    #region Integration Scenarios

    [Theory]
    [InlineData(true, CustomThemeMode.AutoDiscover)] // All themes
    [InlineData(true, CustomThemeMode.None)] // Predefined only
    [InlineData(false, CustomThemeMode.AutoDiscover)] // Custom with specific predefined
    public void Scenario_DifferentConfigurations_AreValid(bool includePredefined, CustomThemeMode customMode)
    {
        // Arrange
        var options = new ThemeSwitcherOptions
        {
            IncludeAllPredefinedThemes = includePredefined,
            CustomThemeMode = customMode
        };

        // Act & Assert
        if (includePredefined || customMode != CustomThemeMode.None)
            Should.NotThrow(() => options.Validate("Custom"));
    }

    [Fact]
    public void Scenario_CustomOnlyWithExplicitThemes_IsValid()
    {
        // Arrange - Custom only with explicit themes provided
        var customTheme1 = new TestCustomTheme("test1.css");
        var customTheme2 = new TestCustomTheme("test2.css");

        var options = new ThemeSwitcherOptions()
            .WithThemes(customTheme1, customTheme2)
            .WithCustomThemes(CustomThemeMode.ExplicitOnly);

        // Act & Assert
        Should.NotThrow(() => options.Validate(customTheme1.GetThemeName()));
    }

    [Fact]
    public void Scenario_ExcludeMultipleThemes_WorksCorrectly()
    {
        // Arrange
        var futuristicTheme = Theme.Futuristic;
        var desertTheme = Theme.Desert;
        var forestTheme = Theme.Forest;

        var options = new ThemeSwitcherOptions()
            .ExcludeThemes(futuristicTheme, desertTheme, forestTheme);

        // Act
        var excluded = options.ExcludedThemes;

        // Assert
        excluded.Count.ShouldBe(3);
        excluded.ShouldContain(futuristicTheme);
        excluded.ShouldContain(desertTheme);
        excluded.ShouldContain(forestTheme);
    }

    [Fact]
    public void Scenario_MixPredefinedAndCustom_WorksCorrectly()
    {
        // Arrange
        var darkTheme = Theme.Dark;
        var lightTheme = Theme.Light;
        var customTheme = new TestCustomTheme("test.css");
        var options = new ThemeSwitcherOptions()
            .WithThemes(darkTheme, lightTheme, customTheme)
            .WithCustomThemes(CustomThemeMode.ExplicitOnly);

        // Act
        var included = options.IncludedThemes;

        // Assert
        included.Count.ShouldBe(3);
        included.ShouldContain(darkTheme);
        included.ShouldContain(lightTheme);
        included.ShouldContain(customTheme);
    }

    [Fact]
    public void Scenario_CustomThemeModes_WorkCorrectly()
    {
        // Test None mode
        var noneOptions = ThemeSwitcherOptions.PredefinedOnly();
        noneOptions.CustomThemeMode.ShouldBe(CustomThemeMode.None);

        // Test ExplicitOnly mode
        var explicitOptions = new ThemeSwitcherOptions().WithCustomThemes(CustomThemeMode.ExplicitOnly);
        explicitOptions.CustomThemeMode.ShouldBe(CustomThemeMode.ExplicitOnly);

        // Test AutoDiscover mode (default)
        var autoOptions = ThemeSwitcherOptions.All();
        autoOptions.CustomThemeMode.ShouldBe(CustomThemeMode.AutoDiscover);
    }

    #endregion Integration Scenarios

    private class TestCustomTheme(string fileName) : Theme(fileName);
}