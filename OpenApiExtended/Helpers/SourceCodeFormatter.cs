
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace OpenApiExtended.Helpers;

internal static class SourceCodeFormatter
{
    internal static string ToFormattedCSharp(this string csharpText, bool fileScopedNamespace)
    {
        if (csharpText == null) throw new ArgumentNullException(nameof(csharpText));
        if (string.IsNullOrWhiteSpace(csharpText))
        {
            return csharpText;
        }
        return csharpText.Contains("public record") ? csharpText.ToFormattedCSharpRecord(fileScopedNamespace) : csharpText.ToFormattedCSharpClass(fileScopedNamespace);
    }

    internal static string ToFormattedCSharpClass(this string csharpText, bool fileScopedNamespace)
    {
        if (csharpText == null) throw new ArgumentNullException(nameof(csharpText));
        if (string.IsNullOrWhiteSpace(csharpText))
        {
            return csharpText;
        }
        var result = new StringBuilder();
        var isFirstTime = true;
        var lines = csharpText.Trim().Split(new[] { "{ get; set; }", "{", "}", ";" }, StringSplitOptions.None).Where(x => x.Trim() != string.Empty).ToList();
        foreach (var line in lines)
        {
            var newLine = line.Trim().RemoveMoreWhiteSpaces();
            if (newLine.StartsWith("using"))
            {
                result.AppendLine(newLine + ";");
            }
            else if (newLine.StartsWith("namespace"))
            {
                if (fileScopedNamespace)
                {
                    result.AppendLine($"{newLine};");
                }
                else
                {
                    result.AppendLine(newLine);
                    result.AppendLine("{");
                }
            }
            else if (newLine.StartsWith("public class"))
            {
                if (!isFirstTime)
                {
                    result.AppendLine(fileScopedNamespace ? "}" : "    }");
                }

                result.AppendLine(fileScopedNamespace ? $"{newLine}" : $"    {newLine}");
                result.AppendLine(fileScopedNamespace ? "{" : "    {");

                isFirstTime = false;
            }
            else
            {
                result.AppendLine(fileScopedNamespace ? $"    {newLine} {{ get; set; }}" : $"        {newLine} {{ get; set; }}");
            }
        }
        result.AppendLine(fileScopedNamespace ? "}" : "    }");
        if (!fileScopedNamespace)
            result.AppendLine("}");

        return result.ToString();
    }

    internal static string ToFormattedCSharpRecord(this string csharpText, bool fileScopedNamespace)
    {
        if (csharpText == null) throw new ArgumentNullException(nameof(csharpText));
        if (string.IsNullOrWhiteSpace(csharpText))
        {
            return csharpText;
        }

        var result = new StringBuilder();
        var isFirstTime = true;
        var lines = csharpText.Trim().Split(new[] { "{", "}", "( ", " )", ",", ";" }, StringSplitOptions.None).Where(x => x.Trim() != string.Empty).ToList();
        var index = 0;
        foreach (var line in lines)
        {
            var newLine = line.Trim().RemoveMoreWhiteSpaces();
            if (newLine.StartsWith("using"))
            {
                result.AppendLine(newLine + ";");
            }
            else if (newLine.StartsWith("namespace"))
            {
                if (fileScopedNamespace)
                {
                    result.AppendLine($"{newLine};");
                }
                else
                {
                    result.AppendLine(newLine);
                    result.AppendLine("{");
                }
            }
            else if (newLine.StartsWith("public record"))
            {
                if (!isFirstTime)
                {
                    result.AppendLine(fileScopedNamespace ? ");" : "    );");
                }
                result.AppendLine(fileScopedNamespace ? $"{newLine}(" : $"    {newLine}(");
                isFirstTime = false;
            }
            else
            {
                var needsComma = index + 1 != lines.Count && !lines[index + 1].Trim().RemoveMoreWhiteSpaces().Contains("public record");
                var comma = needsComma ? "," : string.Empty;
                result.AppendLine(fileScopedNamespace ? $"    {newLine}(" : $"        {newLine}{comma}");
            }
            index++;
        }
        result.AppendLine(fileScopedNamespace ? ");" : "    );");
        if (!fileScopedNamespace)
            result.AppendLine("}");

        return result.ToString();
    }

    internal static string ToFormattedJson(this string jsonText)
    {
        if (jsonText == null) throw new ArgumentNullException(nameof(jsonText));

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            return jsonText;
        }
        jsonText = jsonText.Trim().Trim(',').Trim();
        var parsedJson = JsonDocument.Parse(jsonText, new JsonDocumentOptions { AllowTrailingCommas = true });
        var result = JsonSerializer.Serialize(parsedJson, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }); 
        return result;
    }

    internal static string ToFormattedTypeScript(this string typescriptText)
    {
        if (typescriptText == null) throw new ArgumentNullException(nameof(typescriptText));
        if (string.IsNullOrWhiteSpace(typescriptText))
        {
            return typescriptText;
        }

        var isInterfaceBased = typescriptText.Contains("export interface");
        var result = new StringBuilder();
        var isFirstTime = true;
        var lines = typescriptText.Trim().Split(';', '{', '}').Where(x => x.Trim() != string.Empty).ToList();
        foreach (var line in lines)
        {
            var newLine = line.Trim().RemoveMoreWhiteSpaces();
            var hasInterfaceKeyword = newLine.Contains("export interface");
            var hasTypeKeyword = newLine.Contains("export type");
            if (isInterfaceBased)
            {
                if (hasInterfaceKeyword)
                {
                    if (!isFirstTime)
                    {
                        result.AppendLine("}");
                    }
                    result.AppendLine(newLine);
                    result.AppendLine("{");
                }
                else
                {
                    result.AppendLine($"    {newLine};");
                }
            }
            else
            {
                if (hasTypeKeyword)
                {
                    if (!isFirstTime)
                    {
                        result.AppendLine("};");
                    }
                    result.AppendLine($"{newLine} {{");
                }
                else
                {
                    result.AppendLine($"    {newLine};");
                }
            }

            isFirstTime = false;
        }
        result.AppendLine("};");

        return result.ToString().Trim();
    }
}