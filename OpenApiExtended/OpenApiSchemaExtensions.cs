// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using Humanizer;
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using OpenApiExtended.Helpers;
using OpenApiExtended.Models;
using OpenApiExtended.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static IEnumerable<OpenApiSchemaInfo?> Flatten(this OpenApiSchema openApiSchema, string keySeparator = ".", string root = "$")
    {
        if (openApiSchema == null) throw new ArgumentNullException(nameof(openApiSchema));
        var stack = new Stack<OpenApiSchemaInfo>();
        var parents = new Stack<string>();
        // Initial state of counter.
        var counterToPop = -1;
        // First item (root) to add.
        stack.Push(new OpenApiSchemaInfo(keySeparator)
        {
            Name = root,
            Type = openApiSchema.Type,
            Format = openApiSchema.Format,
            Schema = openApiSchema
        });
        var newCurrent = new List<OpenApiSchemaInfo>();
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            // Object is parent maker.
            if (string.Equals(current.Type, "object", StringComparison.OrdinalIgnoreCase))
            {
                // Push root if nothing is available in the stack.
                parents.Push(parents.Count == 0 ? root : current.Name);
            }
            // If it is not Object or Array means every iteration we are closing to finish
            // traversing of a parent.
            if (!string.Equals(current.Type, "object", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(current.Type, "array", StringComparison.OrdinalIgnoreCase))
            {
                --counterToPop;
            }
            // If counter is zero, we finished traverse on current parent so we should pop it. Reset
            // counter to initial state until next time.
            if (counterToPop == 0)
            {
                parents.Pop();
                counterToPop = -1;
            }
            yield return current;

            // We should reverse stack to show them correctly in the list.
            var parentsWithCorrectOrder = parents.Reverse().ToList();
            switch (current.Schema.Type.ToLower())
            {
                case "object":
                    var props = current.Schema.Properties.Reverse().ToList();
                    // We should save count of properties to make sure about the parent of them.
                    counterToPop = props.Count;
                    newCurrent.AddRange(props.Select(prop => new OpenApiSchemaInfo(keySeparator) { Name = prop.Key, Type = prop.Value.Type, Schema = prop.Value, Parents = parentsWithCorrectOrder }));
                    break;

                case "array":
                    var item = new OpenApiSchemaInfo(keySeparator)
                    {
                        Name = current.Name,
                        Type = current.Schema.Items.Type,
                        Format = current.Schema.Items.Format,
                        Schema = current.Schema.Items,
                        Parents = parentsWithCorrectOrder
                    };
                    newCurrent.Add(item);
                    break;
            }
            newCurrent.ForEach(stack.Push);
            newCurrent.Clear();
        }
    }

    public static string GetFormat(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Format;
    }

    public static OpenApiSchema? GetReference(this OpenApiSchema openApiSchema, OpenApiDocument openApiDocument)
    {
        return openApiSchema.GetReference(openApiDocument, out _);
    }

    public static OpenApiSchema? GetReference(this OpenApiSchema openApiSchema, OpenApiDocument openApiDocument, out string? referenceId)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }

        if (!openApiSchema.HasReference())
        {
            referenceId = null;
            return null;
        }

        var refId = openApiSchema.Reference.Id;
        referenceId = refId;
        var result = openApiDocument.GetComponentsSchema(x => x == refId).FirstOrDefault();
        return result;
    }

    public static IEnumerable<string> GetRequired(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Required.Select(x => x);
    }

    public static IEnumerable<string> GetRequired(this OpenApiSchema openApiSchema, Func<string, bool> predicate)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Required.Where(predicate).Select(x => x);
    }

    public static (string type, string format) GetTypeAndFormat(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null) throw new ArgumentNullException(nameof(openApiSchema));
        return (openApiSchema.Type, openApiSchema.Format);
    }

    public static string GetTypeAsString(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Type;
    }

    public static bool HasReference(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Reference != null;
    }

    public static bool HasRequired(this OpenApiSchema openApiSchema, Func<string, bool> predicate)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Required.Any(predicate);
    }

    public static bool IsArray(this OpenApiSchema schema)
    {
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        return string.Equals(schema.Type, "array", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsEmptyObject(this OpenApiSchema openApiSchema)
    {
        return openApiSchema.IsEmptyObject(out _);
    }

    public static bool IsEmptyObject(this OpenApiSchema openApiSchema, out OpenApiReference? openApiReference)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        var status = openApiSchema.Reference != null
               && (openApiSchema.Properties == null || openApiSchema.Properties.Count == 0)
               && openApiSchema.Items == null
               && string.Equals(openApiSchema.Type, "object", StringComparison.OrdinalIgnoreCase)
            ;

        openApiReference = status ? openApiSchema.Reference : null;
        return status;
    }

    public static bool IsObject(this OpenApiSchema schema)
    {
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        return string.Equals(schema.Type, "object", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsObjectOrEmptyObjectOrArray(this OpenApiSchema schema)
    {
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        return schema.IsObject() || schema.IsEmptyObject() || schema.IsArray();
    }

    public static bool IsPrimitive(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return !openApiSchema.IsObjectOrEmptyObjectOrArray() && (!string.IsNullOrEmpty(openApiSchema.Type) || !string.IsNullOrEmpty(openApiSchema.Format));
    }

    public static bool IsUnrecognizable(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return !openApiSchema.IsArray() && !openApiSchema.IsObject() && string.IsNullOrEmpty(openApiSchema.Type) && string.IsNullOrEmpty(openApiSchema.Format) && !openApiSchema.HasReference();
    }

    public static string ToCSharp(this OpenApiSchema openApiSchema, CSharpGeneratorSettings settings)
    {
        return settings.TypeStyle switch
        {
            CSharpTypeStyle.Class => openApiSchema.ToCSharpClass(settings),
            CSharpTypeStyle.Record => openApiSchema.ToCSharpRecord(settings),
            _ => throw new ArgumentOutOfRangeException(nameof(settings.TypeStyle), settings.TypeStyle, null)
        };
    }

    public static IEnumerable<OpenApiSchemaIR> ToIntermediateRepresentation(this OpenApiSchema openApiSchema, Func<string, string, object>? defaultValueProvider = null, string separator = ".", string prefix = "")
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        var schema = new List<OpenApiSchemaIR>();
        var flatJson = openApiSchema.ToJsonElement(defaultValueProvider)?.Flatten(separator, prefix);
        if (flatJson == null) return schema;

        foreach (var item in flatJson)
        {
            schema.Add(new OpenApiSchemaIR
            {
                Key = item.Key,
                SeparatedKey = item.Key.Split(new[] { separator }, StringSplitOptions.None),
                Value = item.Value.Value.ToString(),
                ValueKind = item.Value.Kind,
                CSharpKind = item.Value.Kind.ToCSharpType(),
                TypeScriptKind = item.Value.Kind.ToTypeScriptType()
            });
        }

        return schema;
    }

    public static JsonElement? ToJsonElement(this OpenApiSchema openApiSchema, Func<string, string, object>? defaultValueProvider = null)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        var json = openApiSchema.ToJsonExample(defaultValueProvider);
        if (string.IsNullOrEmpty(json)) return null;

        var jsonNode = JsonNode.Parse(json);
        return jsonNode.Deserialize<JsonElement>();
    }

    public static string ToJsonExample(this OpenApiSchema schema, Func<string, string, object>? defaultValueProvider = null)
    {
        return GenerateJsonExample(schema, new Dictionary<string, OpenApiSchema>(), defaultValueProvider).ToFormattedJson();
    }

    private static string GenerateJsonExample(this OpenApiSchema schema, IReadOnlyDictionary<string, OpenApiSchema> knownSchemas, Func<string, string, object>? defaultValueProvider = null)
    {
        using var stream = new MemoryStream();
        using var utfWriter = new Utf8JsonWriter(stream);

        WriteJsonExample(utfWriter, schema);
        utfWriter.Flush();

        return Encoding.UTF8.GetString(stream.ToArray());

        void WriteJsonExample(Utf8JsonWriter writer, OpenApiSchema s)
        {
            if (s.Reference != null && knownSchemas.ContainsKey(s.Reference.Id))
            {
                WriteJsonExample(writer, knownSchemas[s.Reference.Id]);
                return;
            }

            if (s.Default != null)
            {
                writer.WriteStringValue(s.Default.ToString());
                return;
            }

            if (defaultValueProvider != null)
            {
                var replacementDefault = defaultValueProvider(s.Type, s.Format);
                if (replacementDefault is string strVal)
                {
                    writer.WriteStringValue(strVal);
                    return;
                }
                if (replacementDefault is bool boolVal)
                {
                    writer.WriteBooleanValue(boolVal);
                    return;
                }
                writer.WriteNumberValue(Convert.ToDecimal(replacementDefault));
                return;
            }

            switch (s.Type)
            {
                case "string":
                    string exampleString = "example_string";
                    switch (s.Format)
                    {
                        case "date":
                            exampleString = DateTime.Now.ToString("yyyy-MM-dd");
                            break;
                        case "date-time":
                            exampleString = DateTime.Now.ToString("o");
                            break;
                        case "password":
                            exampleString = "password123!";
                            break;
                        case "byte":
                            exampleString = Convert.ToBase64String(new byte[] { 1, 2, 3 });
                            break;
                        case "binary":
                            exampleString = Encoding.ASCII.GetString(new byte[] { 1, 2, 3 });
                            break;
                        case "uuid":
                        case "guid":
                            exampleString = Guid.NewGuid().ToString();
                            break;
                        case "email":
                            exampleString = "example@example.com";
                            break;
                        case "hostname":
                            exampleString = "example.com";
                            break;
                        case "ipv4":
                            exampleString = "192.168.1.1";
                            break;
                        case "ipv6":
                            exampleString = "2001:0db8:85a3:0000:0000:8a2e:0370:7334";
                            break;
                        case "uri":
                            exampleString = "https://www.example.com/path?query=value";
                            break;
                        case "phone":
                            exampleString = "+1-123-456-7890";
                            break;
                        case "hexcolor":
                            exampleString = "#FF5733";
                            break;
                    }
                    if (s.MinLength.HasValue)
                    {
                        exampleString = exampleString.PadRight(s.MinLength.Value, 'x');
                    }
                    if (s.MaxLength.HasValue && exampleString.Length > s.MaxLength.Value)
                    {
                        exampleString = exampleString.Substring(0, s.MaxLength.Value);
                    }
                    writer.WriteStringValue(exampleString);
                    break;

                case "number":
                    var exampleNumber = 123.45678M;
                    switch (s.Format)
                    {
                        case "float":
                            exampleNumber = 123.45M;
                            break;
                        case "double":
                            exampleNumber = 123.4567890123456789M;
                            break;
                        case "currency":
                            writer.WritePropertyName("amount");
                            writer.WriteNumberValue(123.45M);
                            writer.WritePropertyName("code");
                            writer.WriteStringValue("USD");
                            break;
                    }
                    if (s.Minimum.HasValue)
                    {
                        exampleNumber = Math.Max(exampleNumber, s.Minimum.Value);
                    }
                    if (s.Maximum.HasValue)
                    {
                        exampleNumber = Math.Min(exampleNumber, s.Maximum.Value);
                    }
                    writer.WriteNumberValue(exampleNumber);
                    break;

                case "integer":
                    long exampleInt = 123;
                    switch (s.Format)
                    {
                        case "int32":
                            exampleInt = 123;
                            break;
                        case "int64":
                            exampleInt = 1234567890123456789L;
                            break;
                        case "timestamp":
                            exampleInt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            break;
                    }
                    if (s.Minimum.HasValue)
                    {
                        exampleInt = Math.Max(exampleInt, (long)s.Minimum.Value);
                    }
                    if (s.Maximum.HasValue)
                    {
                        exampleInt = Math.Min(exampleInt, (long)s.Maximum.Value);
                    }
                    writer.WriteNumberValue(exampleInt);
                    break;

                case "boolean":
                    writer.WriteBooleanValue(true);
                    break;

                case "array":
                    writer.WriteStartArray();
                    WriteJsonExample(writer, s.Items);
                    writer.WriteEndArray();
                    break;

                case "object":
                    writer.WriteStartObject();
                    switch (s.Format)
                    {
                        case "geo-coordinate":
                            writer.WritePropertyName("lat");
                            writer.WriteNumberValue(40.7128M);
                            writer.WritePropertyName("long");
                            writer.WriteNumberValue(-74.0060M);
                            break;
                        default:
                            var propertiesToGenerate = s.Required?.Count > 0 ? s.Properties.Where(p => s.Required.Contains(p.Key)) : s.Properties;
                            foreach (var property in propertiesToGenerate)
                            {
                                writer.WritePropertyName(property.Key);
                                WriteJsonExample(writer, property.Value);
                            }
                            break;
                    }
                    writer.WriteEndObject();
                    break;
            }
        }
    }
    public static JsonNode? ToJsonNode(this OpenApiSchema openApiSchema, Func<string, string, object>? defaultValueProvider = null)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        var json = openApiSchema.ToJsonExample(defaultValueProvider);
        return string.IsNullOrEmpty(json) ? null : JsonNode.Parse(json);
    }

    public static string ToTypeScript(this OpenApiSchema openApiSchema, TypeScriptGeneratorSettings settings)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        var name = settings.InterfaceOrTypeName.Pascalize();
        var result = string.Empty;
        var items = openApiSchema.Flatten().ToList();

        if (items.Count == 0)
        {
            return string.Empty;
        }

        Regex pattern = new("_____.+?_____", RegexOptions.Compiled);
        var index = 0;
        var typeNeedsSemiColon = settings.TypeStyle == TypeScriptTypeStyle.Type ? ";" : string.Empty;

        foreach (var item in items)
        {
            if (item != null && item.IsRoot())
            {
                result = item.Type switch
                {
                    "object" => $"{GetExportBlock(name, settings.TypeStyle)} {{ {GetAlternativeKey(items.GetRoot() ?? "$")} }}{typeNeedsSemiColon} ",
                    // _ => throw new Exception("Root element is not an object.")
                    _ => string.Empty
                };
            }
            else
            {
                var optional = item?.Schema.GetRequired(x => x == item.Name).Any() == false ? "?" : string.Empty;
                switch (item?.Type)
                {
                    case "array":
                        {
                            var interfaceName = item.Name.Singularize(false).Pascalize();
                            var replace = $"{item.Name}{optional}: {interfaceName}[]; {GetAlternativeKey(item.ParentKey)}";
                            result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                            result += $"{GetExportBlock(interfaceName, settings.TypeStyle)} {{ {GetAlternativeKey(item.Key)} }}{typeNeedsSemiColon} ";
                            break;
                        }
                    case "object":
                        {
                            var prevItemWithSameKeyButArray = items[index - 1]?.Name == item.Name && items[index - 1]?.Type == "array";
                            if (!prevItemWithSameKeyButArray)
                            {
                                var interfaceName = IsSingularizationException(item.Name)
                                    ? item.Name.Pascalize()
                                    : item.Name.Singularize(false).Pascalize();
                                var replace = $"{item.Name}{optional}: {interfaceName}[]; {GetAlternativeKey(item.ParentKey)}";
                                result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                                result += $"{GetExportBlock(interfaceName, settings.TypeStyle)} {{ {GetAlternativeKey(item.Key)} }}{typeNeedsSemiColon} ";
                            }
                            break;
                        }
                }

                if (item != null && item.Type != "array" && item.Type != "object")
                {
                    var tsType = settings.CustomTypeReplacer?.Invoke(item.Schema.Type, item.Schema.Format) ?? OpenApiUtility.GetTypeScriptType(OpenApiUtility.GetOpenApiValueType(item.Schema.Type, item.Schema.Format), settings.UseAny, settings.UseDate);
                    var replace = $"{item.Name}{optional}: {tsType}; {GetAlternativeKey(item.ParentKey)}";
                    result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);

        if (string.IsNullOrEmpty(result))
        {
            return string.Empty;
        }

        result = result.ToFormattedTypeScript();
        return result;

        string GetExportBlock(string value, TypeScriptTypeStyle typeStyle)
        {
            return typeStyle switch
            {
                TypeScriptTypeStyle.Interface => $"export interface {value}",
                TypeScriptTypeStyle.Type => $"export type {value} =",
                _ => throw new ArgumentOutOfRangeException(nameof(settings.TypeStyle), settings.TypeStyle, null)
            };
        }
    }

    public static void Traverse(this OpenApiSchema openApiSchema, Action<OpenApiSchemaInfo> action, string keySeparator = ".", string root = "$")
    {
        var items = openApiSchema.Flatten(keySeparator, root);
        foreach (var item in items)
        {
            if (item != null)
            {
                action?.Invoke(item);
            }
        }
    }

    private static string GetAlternativeKey(this string data) => $"_____{data}_____";

    private static bool IsSingularizationException(this string word)
    {
        var exceptions = new[] { "data" };
        return exceptions.Any(x => string.Equals(x, word, StringComparison.OrdinalIgnoreCase));
    }

    private static string ToCSharpClass(this OpenApiSchema openApiSchema, CSharpGeneratorSettings settings)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        var className = settings.ClassOrRecordName.Pascalize();
        var ns = settings.Namespace.Pascalize();
        var nullableMark = settings.UseNullable ? "?" : string.Empty;
        var result = string.Empty;
        var items = openApiSchema.Flatten().ToList();

        if (items.Count == 0)
        {
            return string.Empty;
        }

        Regex pattern = new("_____.+?_____", RegexOptions.Compiled);
        var index = 0;

        foreach (var item in items)
        {
            if (item != null && item.IsRoot())
            {
                result = item.Type switch
                {
                    "object" => $"public class {className} {{ {GetAlternativeKey(items.GetRoot() ?? "$")} }} ",
                    // _ => throw new Exception("Root element is not an object.")
                    _ => string.Empty
                };
            }
            else
            {
                switch (item?.Type)
                {
                    case "array":
                        {
                            var clsName = item.Name.Singularize(false).Pascalize();
                            var genericType = settings.UseArray ? $"{clsName}[]{nullableMark}" : $"List<{clsName}>{nullableMark}";
                            var replace = $"[JsonPropertyName(\"{item.Name}\")] public {genericType} {item.Name.Pascalize()} {{ get; set; }} {GetAlternativeKey(item.ParentKey)}";
                            result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                            result += $"public class {clsName} {{ {GetAlternativeKey(item.Key)} }} ";
                            break;
                        }
                    case "object":
                        {
                            var prevItemWithSameKeyButArray = items[index - 1]?.Name == item.Name && items[index - 1]?.Type == "array";
                            if (!prevItemWithSameKeyButArray)
                            {
                                var clsName = IsSingularizationException(item.Name)
                                    ? item.Name.Pascalize()
                                    : item.Name.Singularize(false).Pascalize();
                                var genericType = settings.UseArray ? $"{clsName}[]{nullableMark}" : $"List<{clsName}>{nullableMark}";
                                var replace = $"[JsonPropertyName(\"{item.Name}\")] public {genericType} {item.Name.Pascalize()} {{ get; set; }} {GetAlternativeKey(item.ParentKey)}";
                                result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                                result += $"public class {clsName} {{ {GetAlternativeKey(item.Key)} }} ";
                            }
                            break;
                        }
                }

                if (item != null && item.Type != "array" && item.Type != "object")
                {
                    var csType = settings.CustomTypeReplacer?.Invoke(item.Schema.Type, item.Schema.Format) ?? OpenApiUtility.GetCSharpType(OpenApiUtility.GetOpenApiValueType(item.Schema.Type, item.Schema.Format), "object", settings.UseArray, settings.UseDateTime);
                    var replace = $"[JsonPropertyName(\"{item.Name}\")] public {csType}{nullableMark} {item.Name.Pascalize()} {{ get; set; }} {GetAlternativeKey(item.ParentKey)}";
                    result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);

        if (string.IsNullOrEmpty(result))
        {
            return string.Empty;
        }

        result = settings.FileScopedNamespace ? $"namespace {ns}; {result}" : $"namespace {ns} {{ {result} }}";
        var genericNamespace = settings.UseArray ? string.Empty : "using System.Collections.Generic;";
        result = $"using System; {genericNamespace} using System.Text.Json.Serialization; " + result;
        result = result.ToFormattedCSharp(settings.FileScopedNamespace);
        return result;
    }

    private static string ToCSharpRecord(this OpenApiSchema openApiSchema, CSharpGeneratorSettings settings)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        var className = settings.ClassOrRecordName.Pascalize();
        var ns = settings.Namespace.Pascalize();
        var nullableMark = settings.UseNullable ? "?" : string.Empty;
        var result = string.Empty;
        var items = openApiSchema.Flatten().ToList();

        if (items.Count == 0)
        {
            return string.Empty;
        }

        Regex pattern = new("_____.+?_____", RegexOptions.Compiled);
        Regex lastCommaRemover = new(",\\s*\\)", RegexOptions.Compiled);
        var index = 0;

        foreach (var item in items)
        {
            if (item != null && item.IsRoot())
            {
                result = item.Type switch
                {
                    "object" => $"public record {className} ( {GetAlternativeKey(items.GetRoot() ?? "$")} ); ",
                    // _ => throw new Exception("Root element is not an object.")
                    _ => string.Empty
                };
            }
            else
            {
                switch (item?.Type)
                {
                    case "array":
                        {
                            var clsName = item.Name.Singularize(false).Pascalize();
                            var genericType = settings.UseArray ? $"{clsName}[]{nullableMark}" : $"IReadOnlyList<{clsName}>{nullableMark}";
                            var replace = $"[property: JsonPropertyName(\"{item.Name}\")] {genericType} {item.Name.Pascalize()}, {GetAlternativeKey(item.ParentKey)}";
                            result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                            result += $"public record {clsName} ( {GetAlternativeKey(item.Key)} ); ";
                            break;
                        }
                    case "object":
                        {
                            var prevItemWithSameKeyButArray = items[index - 1]?.Name == item.Name && items[index - 1]?.Type == "array";
                            if (!prevItemWithSameKeyButArray)
                            {
                                var clsName = IsSingularizationException(item.Name)
                                    ? item.Name.Pascalize()
                                    : item.Name.Singularize(false).Pascalize();
                                var genericType = settings.UseArray ? $"{clsName}[]{nullableMark}" : $"IReadOnlyList<{clsName}>{nullableMark}";
                                var replace = $"[property: JsonPropertyName(\"{item.Name}\")] {genericType} {item.Name.Pascalize()}, {GetAlternativeKey(item.ParentKey)}";
                                result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                                result += $"public record {clsName} ( {GetAlternativeKey(item.Key)} ); ";
                            }
                            break;
                        }
                }

                if (item != null && item.Type != "array" && item.Type != "object")
                {
                    var csType = settings.CustomTypeReplacer?.Invoke(item.Schema.Type, item.Schema.Format) ?? OpenApiUtility.GetCSharpType(OpenApiUtility.GetOpenApiValueType(item.Schema.Type, item.Schema.Format), "object", settings.UseArray, settings.UseDateTime);
                    var replace = $"[property: JsonPropertyName(\"{item.Name}\")] {csType}{nullableMark} {item.Name.Pascalize()}, {GetAlternativeKey(item.ParentKey)}";
                    result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);

        if (string.IsNullOrEmpty(result))
        {
            return string.Empty;
        }

        result = lastCommaRemover.Replace(result, " )");
        result = settings.FileScopedNamespace ? $"namespace {ns}; {result}" : $"namespace {ns} {{ {result} }}";
        var genericNamespace = settings.UseArray ? string.Empty : "using System.Collections.Generic;";
        result = $"using System; {genericNamespace} using System.Text.Json.Serialization; " + result;
        result = result.ToFormattedCSharp(settings.FileScopedNamespace);
        return result;
    }
}