namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Advanced features for Swagger UI.
/// </summary>
internal static class AdvancedOptions
{
    internal const string PinnableTopbar = nameof(PinnableTopbar);
    internal const string BackToTop = nameof(BackToTop);
    internal const string StickyOperations = nameof(StickyOperations);
    internal const string ExpandOrCollapseAllOperations = nameof(ExpandOrCollapseAllOperations);

    // CSS placeholders
    internal const string StickyOperationsCssPH = "#STICKY_OPERATIONS";

    //private static readonly string[][] s_jsIndependentFeatures = [[StickyOperations, StickyOperationsCssPH]];

    // JS placeholders
    internal const string PinnableTopbarJsPH = "{$PINNABLE_TOPBAR}";
    internal const string BackToTopJsPH = "{$BACK_TO_TOP}";
    internal const string ExpandOrCollapseAllOperationsJsPH = "{$EXPAND_COLLAPSE_ALL_OPERATIONS}";

    private static readonly string[][] s_jsDependentFeatures = [
        [PinnableTopbar, PinnableTopbarJsPH],
        [BackToTop, BackToTopJsPH],
        [ExpandOrCollapseAllOperations, ExpandOrCollapseAllOperationsJsPH]
    ];

    internal static void EnablePinnableTopbar(this IDictionary<string, object> options)
    {
        options.TryAdd(PinnableTopbar, true);
    }

    internal static void EnableBackToTop(this IDictionary<string, object> options)
    {
        options.TryAdd(BackToTop, true);
    }

    internal static void EnableStickyOperations(this IDictionary<string, object> options)
    {
        options.TryAdd(StickyOperations, true);
    }

    internal static void EnableExpandOrCollapseAllOperations(this IDictionary<string, object> options)
    {
        options.TryAdd(ExpandOrCollapseAllOperations, true);
    }

    internal static bool AnyJsFeatureEnabled(IDictionary<string, object> options)
    {
        foreach (var feature in s_jsDependentFeatures)
        {
            var name = feature[0];
            if (options.TryGetValue(name, out object enabled) && (bool)enabled)
                return true;
        }

        return false;
    }

    internal static string Apply(string content, IDictionary<string, object> advancedOptions, string mimeType)
    {
        return mimeType switch
        {
            MimeTypes.Text.Css => ApplyCssFeatures(content, advancedOptions),
            MimeTypes.Text.Javascript => ApplyJsFeatures(content, advancedOptions),
            _ => content
        };
    }

    #region Private

    private static string ApplyCssFeatures(string cssContent, IDictionary<string, object> cssAdvancedOptions)
    {
        if (cssAdvancedOptions.TryGetValue(StickyOperations, out var isEnabled) && isEnabled is true)
            cssContent = cssContent.Replace(StickyOperationsCssPH, string.Empty);

        return cssContent;
    }

    private static string ApplyJsFeatures(string jsContent, IDictionary<string, object> jsAdvancedOptions)
    {
        foreach (var featurePair in s_jsDependentFeatures)
        {
            var featureKey = featurePair[0];
            var jsKey = featurePair[1];

            if (jsAdvancedOptions.TryGetValue(featureKey, out var isEnabled) && isEnabled is bool)
                jsContent = jsContent.Replace(jsKey, isEnabled.ToString().ToLower());
            else
                jsContent = jsContent.Replace(jsKey, false.ToString().ToLower()); // Disabled by default
        }

        return jsContent;
    }

    #endregion Private
}