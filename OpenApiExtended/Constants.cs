using System.Text.RegularExpressions;

namespace OpenApiExtended
{
    internal class Constants
    {
        internal const string RootIndicator = "$";
        internal const string ArrayItemFormatSeparator = ">";
        internal static readonly Regex ArrayItemRegex = new(@"\-\>\[(.+)\]", RegexOptions.Compiled);
    }
}
