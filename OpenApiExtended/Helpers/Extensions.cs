// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenApiExtended.Helpers;

internal static class Extensions
{
    internal static string? GetDescription(this Enum @enum, bool returnEnumNameInsteadOfNull = false)
    {
        if (@enum == null) throw new ArgumentNullException(nameof(@enum));

        return
            @enum
                .GetType()
                .GetMember(@enum.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description
            ?? (!returnEnumNameInsteadOfNull ? null : @enum.ToString());
    }

    internal static string RemoveMoreWhiteSpaces(this string text)
    {
        return Regex.Replace(text, @"\s+", " ");
    }

    internal static string ReplaceFirst(this string text, string search, string replace)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        int pos = text.IndexOf(search, StringComparison.Ordinal);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
}