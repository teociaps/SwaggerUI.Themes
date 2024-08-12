namespace AspNetCore.Swagger.Themes;

/// <summary>
/// Standard MIME type constants.
/// </summary>
internal static class MimeTypes
{
    /// <summary>
    /// MIME type constants for <c>text/*</c> types.
    /// </summary>
    internal static class Text
    {
        private const string _Prefix = "text/";

        /// <summary>
        /// <c>text/css</c>
        /// </summary>
        internal const string Css = _Prefix + "css";

        /// <summary>
        /// <c>text/javascript</c>
        /// </summary>
        internal const string Javascript = _Prefix + "javascript";
    }
}