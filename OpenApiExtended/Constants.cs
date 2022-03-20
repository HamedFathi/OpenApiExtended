using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenApiExtended
{
    internal class Constants
    {
        internal const string ArrayItemSeparator = "...[";
        internal const string ArrayItemFormatSeparator = ".";
        internal static readonly Regex ArrayItemRegex = new Regex(@"(.+)\.\.\.\[(.+)\]", RegexOptions.Compiled);
    }
}
