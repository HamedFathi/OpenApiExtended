// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using OpenApiExtended.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static bool IsEmptyJsonObject(this string jsonText)
    {
        return Regex.Replace(jsonText.Trim(), @"\s+", "").Trim() == "{}";
    }
    public static bool IsJsonStringValue(this string jsonValue, out string rawValue, bool trim = false)
    {
        var val = trim ? jsonValue.Trim() : jsonValue;
        if (val.StartsWith("\"") && val.EndsWith("\""))
        {
            rawValue = val.Trim('"');
            return true;
        }
        rawValue = val;
        return false;
    }

    public static string ReplaceEmptyObject(this string jsonText, Func<string, string> replacer)
    {
        var lines = jsonText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.Contains("{}") && line.Contains(":"))
            {
                var regex = new Regex(@"\""(.+)\""");
                var parts = line.Split(':');
                var match = regex.Match(parts[0]);
                if (!match.Success)
                {
                    throw new Exception($"Json key {parts[0].Trim()} is not detectable.");
                }
                var key = regex.Match(parts[0]).Groups[1].Value;
                var value = replacer(key);
                var newLine = line.Replace("{}", value);
                sb.AppendLine(newLine);
            }
            else
            {
                sb.AppendLine(line);
            }
        }
        return sb.ToString();
    }
    public static string ReplaceEmptyArray(this string jsonText, Func<string, string> replacer)
    {
        var lines = jsonText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.Contains("[]") && line.Contains(":"))
            {
                var regex = new Regex(@"\""(.+)\""");
                var parts = line.Split(':');
                var match = regex.Match(parts[0]);
                if (!match.Success)
                {
                    throw new Exception($"Json key {parts[0].Trim()} is not detectable.");
                }
                var key = regex.Match(parts[0]).Groups[1].Value;
                var value = replacer(key);
                var newLine = line.Replace("[]", value);
                sb.AppendLine(newLine);
            }
            else
            {
                sb.AppendLine(line);
            }
        }
        return sb.ToString();
    }
    public static bool IsOpenApiDocument(this object obj)
    {
        return obj is OpenApiDocument;
    }

    public static bool IsOpenApiExtensible(this object obj)
    {
        return obj is IOpenApiExtensible;
    }

    public static bool IsOpenApiMediaType(this object obj)
    {
        return obj is OpenApiMediaType;
    }

    public static bool IsOpenApiOperation(this object obj)
    {
        return obj is OpenApiOperation;
    }

    public static bool IsOpenApiParameter(this object obj)
    {
        return obj is OpenApiParameter;
    }

    public static bool IsOpenApiPathItem(this object obj)
    {
        return obj is OpenApiPathItem;
    }

    public static bool IsOpenApiPaths(this object obj)
    {
        return obj is OpenApiPaths;
    }

    public static bool IsOpenApiRequestBody(this object obj)
    {
        return obj is OpenApiRequestBody;
    }

    public static bool IsOpenApiResponse(this object obj)
    {
        return obj is OpenApiResponse;
    }

    public static bool IsOpenApiResponses(this object obj)
    {
        return obj is OpenApiResponses;
    }

    public static bool IsOpenApiSchema(this object obj)
    {
        return obj is OpenApiSchema;
    }

    public static bool IsOpenApiServer(this object obj)
    {
        return obj is OpenApiServer;
    }

    public static string ToCSharpType(this JsonValueKind kind)
    {
        return kind switch
        {
            JsonValueKind.Undefined => "object",
            JsonValueKind.Object => "Dictionary<string, object>",
            JsonValueKind.Array => "List<object>",
            JsonValueKind.String => "string",
            JsonValueKind.Number => "double", // or "decimal"
            JsonValueKind.True => "bool",
            JsonValueKind.False => "bool",
            JsonValueKind.Null => "object",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }

    public static string ToTypeScriptType(this JsonValueKind kind)
    {
        return kind switch
        {
            JsonValueKind.Undefined => "any",
            JsonValueKind.Object => "{ [key: string]: any }",
            JsonValueKind.Array => "any[]",
            JsonValueKind.String => "string",
            JsonValueKind.Number => "number",
            JsonValueKind.True => "boolean",
            JsonValueKind.False => "boolean",
            JsonValueKind.Null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }

    internal static IDictionary<string, (JsonElement Value, JsonValueKind Kind)> Flatten(this JsonElement jsonElement, string separator = ".", string prefix = "")
    {
        var dictionary = new Dictionary<string, (JsonElement, JsonValueKind)>();

        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in jsonElement.EnumerateObject())
                {
                    var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}{separator}{property.Name}";

                    switch (property.Value.ValueKind)
                    {
                        case JsonValueKind.Object:
                        case JsonValueKind.Array:
                            foreach (var nested in Flatten(property.Value, separator, key))
                            {
                                dictionary.Add(nested.Key, nested.Value);
                            }
                            break;
                        default:
                            dictionary.Add(key, (property.Value, property.Value.ValueKind));
                            break;
                    }
                }
                break;
            case JsonValueKind.Array:
                var index = 0;
                foreach (var item in jsonElement.EnumerateArray())
                {
                    var key = $"{prefix}{separator}{index}";
                    switch (item.ValueKind)
                    {
                        case JsonValueKind.Object:
                        case JsonValueKind.Array:
                            foreach (var nested in Flatten(item, separator, key))
                            {
                                dictionary.Add(nested.Key, nested.Value);
                            }
                            break;
                        default:
                            dictionary.Add(key, (item, item.ValueKind));
                            break;
                    }
                    index++;
                }
                break;
            default:
                dictionary.Add(prefix, (jsonElement, jsonElement.ValueKind));
                break;
        }

        return dictionary;
    }

    private static object? ConvertToOpenApiMimeType<T>(string mimeType) where T : Enum
    {
        var result = Utilities.GetEnumDetails<T>().FirstOrDefault(x => x.Description == mimeType);
        if (result == null)
        {
            return null;
        }
        return result.Item;
    }

    private static object? GetOpenApiSchemaDefaultValue(this OpenApiSchema? openApiSchema)
    {
        if (openApiSchema == null) return null;
        if (!openApiSchema.IsPrimitive()) return null;

        var info = OpenApiUtility.GetOpenApiValueType(openApiSchema.Type, openApiSchema.Format);
        return info switch
        {
            OpenApiValueType.Boolean => "true",
            OpenApiValueType.Integer => 0,
            OpenApiValueType.Int32 => 1,
            OpenApiValueType.Int64 => 1,
            OpenApiValueType.Number => 1.0,
            OpenApiValueType.Float => 1.0,
            OpenApiValueType.Double => 1.0,
            OpenApiValueType.String => "\"string\"",
            OpenApiValueType.Date => $"\"{DateTime.Now:yyyy-MM-dd}\"",
            OpenApiValueType.DateTime => $"\"{DateTime.Now:yyyy-MM-ddTHH:mm:ssZ}\"",
            OpenApiValueType.Password => "\"P@ssW0rd\"",
            OpenApiValueType.Byte => "\"ZGVmYXVsdA==\"",
            OpenApiValueType.Email => "\"test@example.com\"",
            OpenApiValueType.Uuid => $"\"{Guid.NewGuid()}\"",
            OpenApiValueType.Uri => "\"https://www.example.com/\"",
            OpenApiValueType.HostName => "\"https://www.domain.example.com/\"",
            OpenApiValueType.IPv4 => "\"127.0.0.1\"",
            OpenApiValueType.IPv6 => "\"::1\"",
            OpenApiValueType.Null => null,
            _ => null
        };
    }
}