using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
// ReSharper disable UnusedMember.Global

namespace OpenApiExtended
{
    public static class OpenApiUtility
    {
        public static IList<string> GetAllTypeScriptTypes(string typescriptSource, bool sorted = false)
        {
            var hashSet = new HashSet<string>();
            var matches = Regex.Matches(typescriptSource, ":(.+);");
            foreach (Match match in matches)
            {
                var group = match.Groups[1].Value;
                var parts =
                    group.Split(' ')
                        .Where(x => x.Trim() != string.Empty && x.Trim() != "|")
                        .Select(x => x.TrimEnd('?'))
                        .ToList();
                hashSet.AddRange(parts);
            }
            var result = hashSet.ToList();
            if (sorted)
            {
                result.Sort();
            }
            return result;
        }
        public static OpenApiValueType GetOpenApiValueType(string type, string format)
        {
            if (!string.IsNullOrEmpty(format))
            {
                if (format.ToLower() == "int32") return OpenApiValueType.Int32;
                if (format.ToLower() == "int64") return OpenApiValueType.Int64;
                if (format.ToLower() == "float") return OpenApiValueType.Float;
                if (format.ToLower() == "double") return OpenApiValueType.Double;
                if (format.ToLower() == "date") return OpenApiValueType.Date;
                if (format.ToLower() == "date-time") return OpenApiValueType.DateTime;
                if (format.ToLower() == "password") return OpenApiValueType.Password;
                if (format.ToLower() == "byte") return OpenApiValueType.Byte;
                if (format.ToLower() == "binary") return OpenApiValueType.Binary;
                if (format.ToLower() == "email") return OpenApiValueType.Email;
                if (format.ToLower() == "uuid") return OpenApiValueType.Uuid;
                if (format.ToLower() == "uri") return OpenApiValueType.Uri;
                if (format.ToLower() == "hostname") return OpenApiValueType.HostName;
                if (format.ToLower() == "ipv4") return OpenApiValueType.IPv4;
                if (format.ToLower() == "ipv6") return OpenApiValueType.IPv6;
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "number") return OpenApiValueType.Number;
                if (type.ToLower() == "integer") return OpenApiValueType.Integer;
                if (type.ToLower() == "string") return OpenApiValueType.String;
                if (type.ToLower() == "boolean") return OpenApiValueType.Boolean;
                if (type.ToLower() == "null") return OpenApiValueType.Null;
                if (type.ToLower() == "array") return OpenApiValueType.Array;
                if (type.ToLower() == "object") return OpenApiValueType.Object;
            }

            return OpenApiValueType.Unknown;
        }
        public static string GetTypeScriptType(OpenApiValueType openApiValueType)
        {
            switch (openApiValueType)
            {
                case OpenApiValueType.Binary:
                case OpenApiValueType.Unknown:
                    return "unknown";

                case OpenApiValueType.Boolean:
                    return "boolean";

                case OpenApiValueType.Integer:
                case OpenApiValueType.Int32:
                case OpenApiValueType.Int64:
                case OpenApiValueType.Number:
                case OpenApiValueType.Float:
                case OpenApiValueType.Double:
                    return "number";

                case OpenApiValueType.Date:
                case OpenApiValueType.DateTime:
                    return "Date";

                case OpenApiValueType.String:
                case OpenApiValueType.Password:
                case OpenApiValueType.Byte:
                case OpenApiValueType.Email:
                case OpenApiValueType.Uuid:
                case OpenApiValueType.Uri:
                case OpenApiValueType.HostName:
                case OpenApiValueType.IPv4:
                case OpenApiValueType.IPv6:
                    return "string";

                case OpenApiValueType.Null:
                    return "null";

                case OpenApiValueType.Array:
                    return "unknown[]";

                case OpenApiValueType.Object:
                    return "object";

                default:
                    throw new ArgumentOutOfRangeException(nameof(openApiValueType), openApiValueType, null);
            }
        }
    }
}