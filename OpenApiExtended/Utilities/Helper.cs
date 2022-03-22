using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OpenApiExtended
{
    internal static class Helper
    {
        private static readonly Pluralizer pluralizer = new Pluralizer();

        internal static string Pluralize(this string word)
        {
            return pluralizer.Pluralize(word);
        }

        internal static string Singularize(this string word)
        {
            return pluralizer.Singularize(word);
        }

        internal static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }
        internal static IEnumerable<EnumMemberInfo> GetEnumInfo<T>() where T : Enum
        {
            var names = Enum.GetNames(typeof(T));
            var values = Enum.GetValues(typeof(T));
            var descriptions = typeof(T).GetMembers()
                 .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                 .Select(x => x.Description)
                 .ToList();

            return names.Select((t, i) => new EnumMemberInfo
            {
                Name = t,
                Value = values.GetValue(i)?.ToString(),
                Description = descriptions[i]
            });
        }

        internal static void AppendLine(this StringBuilder builder, string value, int counter, bool tab = true)
        {
            if (counter <= 0)
            {
                builder.AppendLine(value);
            }
            else
            {
                var space = tab ? new string('\t', counter) : new string(' ', counter);
                builder.AppendLine($"{space}{value}");
            }
        }

        internal static void Append(this StringBuilder builder, string value, int counter, bool tab = true)
        {
            if (counter <= 0)
            {
                builder.Append(value);
            }
            else
            {
                var space = tab ? new string('\t', counter) : new string(' ', counter);
                builder.Append($"{space}{value}");
            }
        }

        internal static string RemoveDuplicateWhiteSpaces(this string text, bool multiLine = false)
        {
            if (multiLine)
                return Regex.Replace(text, @"\s+", " ", RegexOptions.Multiline);

            return Regex.Replace(text, @"\s+", " ");
        }

        internal static string ToFormattedJson(this string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                return jsonText;
            }
            jsonText = jsonText.Trim().Trim(',');
            var parsedJson = JsonDocument.Parse(jsonText, new JsonDocumentOptions() { AllowTrailingCommas = true });
            var result = JsonSerializer.Serialize(parsedJson, new JsonSerializerOptions { WriteIndented = true });
            return result;
        }
        internal static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
        {
            using var enumerator = source.GetEnumerator();
            var queue = new Queue<T>(count + 1);

            while (true)
            {
                if (!enumerator.MoveNext())
                    break;
                queue.Enqueue(enumerator.Current);
                if (queue.Count > count)
                    yield return queue.Dequeue();
            }
        }
        internal static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        internal static string ReplaceLast(string text, string search, string replace)
        {
            int place = text.LastIndexOf(search, StringComparison.Ordinal);

            if (place == -1)
                return text;

            string result = text.Remove(place, search.Length).Insert(place, replace);
            return result;
        }
        internal static bool IsString(this object obj)
        {
            return obj is string;
        }
        internal static T Parse<T>(this Enum @enum, string name, bool ignoreCase = false) where T : Enum
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            return (T)Enum.Parse(typeof(T), name, ignoreCase);
        }
        internal static string GetDescription(this Enum @enum, bool replaceNullWithEnumName = false)
        {
            return
                @enum
                    .GetType()
                    .GetMember(@enum.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? (replaceNullWithEnumName ? null : @enum.ToString());
        }
    }
}
