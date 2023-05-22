// ReSharper disable InconsistentNaming
using System.Text.Json;

namespace OpenApiExtended.Models;

public class OpenApiSchemaIR
{
    public string CSharpKind { get; set; } = null!;
    public string Key { get; set; } = null!;
    public string[] SeparatedKey { get; set; } = null!;
    public string TypeScriptKind { get; set; } = null!;
    public string Value { get; set; } = null!;
    public JsonValueKind ValueKind { get; set; }
}