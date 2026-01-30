using System.Text.RegularExpressions;

namespace BlowUp.Parsers
{
    internal static class BlowUpParserUtilities
    {
        public static readonly Regex ContentAnchorRegex1 = new Regex("\\[\\[([^\\[\\]]+)\\]\\]", RegexOptions.None);

        public static readonly Regex ContentAnchorRegex2 = new Regex("\\[\\[([^\\[\\]]+)::([^\\[\\]]+)\\]\\]", RegexOptions.None);

        public static readonly Regex ContentCodeRegex = new Regex("``(.+?)``", RegexOptions.None);

        public static readonly Regex ContentFontStyleBoldRegex = new Regex("\\*\\*(.+?)\\*\\*", RegexOptions.None);

        public static readonly Regex ContentFontStyleItalicRegex = new Regex("__(.+?)__", RegexOptions.None);

        public static readonly Regex ContentFontStyleStrikethroughRegex = new Regex("~~(.+?)~~", RegexOptions.None);
    }
}