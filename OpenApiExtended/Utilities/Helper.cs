using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace OpenApiExtended
{
    internal static class Helper
    {
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
        internal static string ToFormattedJson(this string jsonText)
        {
            jsonText = jsonText.Trim().Trim(',');
            var parsedJson = JsonDocument.Parse(jsonText, new JsonDocumentOptions() { AllowTrailingCommas = true });
            var result = JsonSerializer.Serialize(parsedJson, new JsonSerializerOptions { WriteIndented = true });
            return result;
        }
    }
}
