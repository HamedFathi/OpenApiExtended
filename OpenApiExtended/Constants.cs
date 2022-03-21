using System.Text.RegularExpressions;

namespace OpenApiExtended
{
    internal class Constants
    {
        internal const string ArrayItemSeparator = "...[";
        internal const string RootIndicator = "$";
        internal const string JsonExampleIndicator = "@@";
        internal const string ArrayItemFormatSeparator = ".";
        internal static readonly Regex ArrayItemRegex = new(@"(.+)\.\.\.\[(.+)\]", RegexOptions.Compiled);
        internal static readonly Regex JsonExampleRegex = new("@@.+?@@", RegexOptions.Compiled);

    }
}
