// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using Humanizer;
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using OpenApiExtended.Helpers;
using OpenApiExtended.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // If it is not Object or Array means every iteration we are closing to finish traversing of a parent.
            if (!string.Equals(current.Type, "object", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(current.Type, "array", StringComparison.OrdinalIgnoreCase))
            {
                --counterToPop;
            }
            // If counter is zero, we finished traverse on current parent so we should pop it.
            // Reset counter to initial state until next time.
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
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        return openApiSchema.Reference != null
            && (openApiSchema.Properties == null || openApiSchema.Properties.Count == 0)
            && openApiSchema.Items == null
            ;
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

    public static string ToCSharp(this OpenApiSchema openApiSchema, string rootClass, string @namespace
        , CSharpResultKind resultKind = CSharpResultKind.Class, bool useNullable = true,
        bool fileScopedNamespace = false, bool useArray = false, bool useDateTimeType = true)
    {
        return resultKind switch
        {
            CSharpResultKind.Class => openApiSchema.ToCSharpClass(rootClass, @namespace, useNullable,
                fileScopedNamespace, useArray, useDateTimeType),
            CSharpResultKind.Record => openApiSchema.ToCSharpRecord(rootClass, @namespace, useNullable,
                fileScopedNamespace, useArray, useDateTimeType),
            _ => throw new ArgumentOutOfRangeException(nameof(resultKind), resultKind, null)
        };
    }

    public static string ToJsonExample(this OpenApiSchema openApiSchema, Func<OpenApiSchemaInfo?, object>? valueProvider = null)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        var result = string.Empty;
        var items = openApiSchema.Flatten().ToList();

        if (items.Count == 0)
        {
            if (!openApiSchema.IsPrimitive()) return string.Empty;
            var value = valueProvider?.Invoke(null).ToString() ?? openApiSchema.GetOpenApiSchemaDefaultValue()?.ToString() ?? string.Empty;
            return value;
        }

        Regex pattern = new("_____.+?_____", RegexOptions.Compiled);
        var index = 0;
        var groupedItems = items.GroupBy(x => x?.ParentKey).ToList();

        foreach (var item in items)
        {
            if (item != null && item.IsRoot())
            {
                result = item.Type switch
                {
                    "object" => $"{{ {GetAlternativeKey(items.GetRoot() ?? "$")} }}",
                    "array" => $"[ {GetAlternativeKey(items.GetRoot() ?? "$")} ]",
                    _ => string.Empty
                };
            }
            else
            {
                switch (item?.Type)
                {
                    case "array":
                        {
                            var replace = $"\"{item.Name}\": [ {GetAlternativeKey(item.Key)} ], {GetAlternativeKey(item.ParentKey)}";
                            result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                            break;
                        }
                    case "object":
                        {
                            var prevItemWithSameKeyButArray = items[index - 1]?.Key == item.Key && items[index - 1]?.Type == "array";
                            if (prevItemWithSameKeyButArray)
                            {
                                result = result.ReplaceFirst(GetAlternativeKey(item.Key), $"{{ {GetAlternativeKey(item.Key)} }}");
                            }
                            else
                            {
                                var refValue = item.Schema.IsEmptyObject() ? "" : GetAlternativeKey(item.Key);
                                var replace = $"\"{item.Name}\": {{ {refValue} }}, {GetAlternativeKey(item.ParentKey)}";
                                result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                            }

                            break;
                        }
                }

                if (item?.Type != "array" && item?.Type != "object")
                {
                    var value = valueProvider?.Invoke(item).ToString() ?? item?.Schema.GetOpenApiSchemaDefaultValue()?.ToString();
                    if (item?.GetParentType(items) == "array" && item.IsArrayItem(items) && value != null)
                    {
                        result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), value);
                    }

                    if (item?.GetParentType(items) == "array" && !item.IsArrayItem(items) && value != null)
                    {
                        var parentGroup = groupedItems.First(x => x.Key == item.ParentKey);
                        var parentGroupData = parentGroup.Select(x => x?.Key).ToList();
                        var parentGroupCount = parentGroupData.Count;
                        var isLastItem = parentGroupData.FindIndex(x => x == item.Key) == parentGroupCount - 1;
                        var reqKey = isLastItem ? "" : GetAlternativeKey(item.ParentKey);

                        var replace = $"\"{item.Name}\": {value}, {reqKey}";
                        result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                    }
                    if (item?.GetParentType(items) == "object" && value != null)
                    {
                        var replace = $"\"{item.Name}\": {value}, {GetAlternativeKey(item.ParentKey)}";
                        result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                    }
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);
        result = result.ToFormattedJson();
        return result;
    }

    public static JsonNode? ToJsonNode(this OpenApiSchema openApiSchema)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }
        var json = openApiSchema.ToJsonExample();
        return JsonNode.Parse(json);
    }

    public static string ToTypeScript(this OpenApiSchema openApiSchema, string name,
            TypeScriptResultKind resultKind = TypeScriptResultKind.Interface, bool useAnyType = false, bool useDateType = true)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        name = name.Pascalize();
        var result = string.Empty;
        var items = openApiSchema.Flatten().ToList();

        if (items.Count == 0)
        {
            return string.Empty;
        }

        Regex pattern = new("_____.+?_____", RegexOptions.Compiled);
        var index = 0;
        var typeNeedsSemiColon = resultKind == TypeScriptResultKind.Type ? ";" : string.Empty;

        foreach (var item in items)
        {
            if (item != null && item.IsRoot())
            {
                result = item.Type switch
                {
                    "object" => $"{GetExportBlock(name, resultKind)} {{ {GetAlternativeKey(items.GetRoot() ?? "$")} }}{typeNeedsSemiColon} ",
                    _ => throw new Exception("Root element is not an object.")
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
                            result += $"{GetExportBlock(interfaceName, resultKind)} {{ {GetAlternativeKey(item.Key)} }}{typeNeedsSemiColon} ";
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
                                result += $"{GetExportBlock(interfaceName, resultKind)} {{ {GetAlternativeKey(item.Key)} }}{typeNeedsSemiColon} ";
                            }
                            break;
                        }
                }

                if (item != null && item.Type != "array" && item.Type != "object")
                {
                    var tsType = OpenApiUtility.GetTypeScriptType(OpenApiUtility.GetOpenApiValueType(item.Schema.Type, item.Schema.Format), useAnyType, useDateType);
                    var replace = $"{item.Name}{optional}: {tsType}; {GetAlternativeKey(item.ParentKey)}";
                    result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);
        result = result.ToFormattedTypeScript();
        return result;

        string GetExportBlock(string value, TypeScriptResultKind kind)
        {
            return kind switch
            {
                TypeScriptResultKind.Interface => $"export interface {value}",
                TypeScriptResultKind.Type => $"export type {value} =",
                _ => throw new ArgumentOutOfRangeException(nameof(resultKind), resultKind, null)
            };
        }
    }

    public static void Traverse(this OpenApiSchema openApiSchema, Action<OpenApiSchemaInfo> action, string keySeparator = ".",
                        string root = "$")
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

    private static string ToCSharpClass(this OpenApiSchema openApiSchema, string rootClass, string @namespace, bool useNullable = true, bool fileScopedNamespace = false, bool useArray = false, bool useDateTimeType = true)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        var className = rootClass.Pascalize();
        @namespace = @namespace.Pascalize();
        var nullableMark = useNullable ? "?" : string.Empty;
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
                    _ => throw new Exception("Root element is not an object.")
                };
            }
            else
            {
                switch (item?.Type)
                {
                    case "array":
                        {
                            var clsName = item.Name.Singularize(false).Pascalize();
                            var genericType = useArray ? $"{clsName}[]{nullableMark}" : $"List<{clsName}>{nullableMark}";
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
                                var genericType = useArray ? $"{clsName}[]{nullableMark}" : $"List<{clsName}>{nullableMark}";
                                var replace = $"[JsonPropertyName(\"{item.Name}\")] public {genericType} {item.Name.Pascalize()} {{ get; set; }} {GetAlternativeKey(item.ParentKey)}";
                                result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                                result += $"public class {clsName} {{ {GetAlternativeKey(item.Key)} }} ";
                            }
                            break;
                        }
                }

                if (item != null && item.Type != "array" && item.Type != "object")
                {
                    var csType = OpenApiUtility.GetCSharpType(OpenApiUtility.GetOpenApiValueType(item.Schema.Type, item.Schema.Format), "object", useArray, useDateTimeType);
                    var replace = $"[JsonPropertyName(\"{item.Name}\")] public {csType}{nullableMark} {item.Name.Pascalize()} {{ get; set; }} {GetAlternativeKey(item.ParentKey)}";
                    result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);
        result = fileScopedNamespace ? $"namespace {@namespace}; {result}" : $"namespace {@namespace} {{ {result} }}";
        var genericNamespace = useArray ? string.Empty : "using System.Collections.Generic;";
        result = $"using System; {genericNamespace} using System.Text.Json.Serialization; " + result;
        result = result.ToFormattedCSharp(fileScopedNamespace);
        return result;
    }

    private static string ToCSharpRecord(this OpenApiSchema openApiSchema, string rootClass, string @namespace,
            bool useNullable = true, bool fileScopedNamespace = false, bool useArray = false, bool useDateTimeType = true)
    {
        if (openApiSchema == null)
        {
            throw new ArgumentNullException(nameof(openApiSchema));
        }

        var className = rootClass.Pascalize();
        @namespace = @namespace.Pascalize();
        var nullableMark = useNullable ? "?" : string.Empty;
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
                    _ => throw new Exception("Root element is not an object.")
                };
            }
            else
            {
                switch (item?.Type)
                {
                    case "array":
                        {
                            var clsName = item.Name.Singularize(false).Pascalize();
                            var genericType = useArray ? $"{clsName}[]{nullableMark}" : $"IReadOnlyList<{clsName}>{nullableMark}";
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
                                var genericType = useArray ? $"{clsName}[]{nullableMark}" : $"IReadOnlyList<{clsName}>{nullableMark}";
                                var replace = $"[property: JsonPropertyName(\"{item.Name}\")] {genericType} {item.Name.Pascalize()}, {GetAlternativeKey(item.ParentKey)}";
                                result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                                result += $"public record {clsName} ( {GetAlternativeKey(item.Key)} ); ";
                            }
                            break;
                        }
                }

                if (item != null && item.Type != "array" && item.Type != "object")
                {
                    var csType = OpenApiUtility.GetCSharpType(OpenApiUtility.GetOpenApiValueType(item.Schema.Type, item.Schema.Format), "object", useArray, useDateTimeType);
                    var replace = $"[property: JsonPropertyName(\"{item.Name}\")] {csType}{nullableMark} {item.Name.Pascalize()}, {GetAlternativeKey(item.ParentKey)}";
                    result = result.ReplaceFirst(GetAlternativeKey(item.ParentKey), replace);
                }
            }
            index++;
        }
        result = pattern.Replace(result, string.Empty);
        result = lastCommaRemover.Replace(result, " )");
        result = fileScopedNamespace ? $"namespace {@namespace}; {result}" : $"namespace {@namespace} {{ {result} }}";
        var genericNamespace = useArray ? string.Empty : "using System.Collections.Generic;";
        result = $"using System; {genericNamespace} using System.Text.Json.Serialization; " + result;
        result = result.ToFormattedCSharp(fileScopedNamespace);
        return result;
    }
}