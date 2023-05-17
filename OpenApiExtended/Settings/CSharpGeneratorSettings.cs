using OpenApiExtended.Enums;
using System;

namespace OpenApiExtended.Settings;

public class CSharpGeneratorSettings
{
    public string ClassOrRecordName { get; set; } = "Root";
    public Func<string, string, string>? CustomTypeReplacer { get; set; }
    public bool FileScopedNamespace { get; set; } = false;
    public string Namespace { get; set; } = "RootNameSpace";
    public CSharpTypeStyle TypeStyle { get; set; } = CSharpTypeStyle.Class;
    public bool UseArray { get; set; } = false;
    public bool UseDateTime { get; set; } = true;
    public bool UseNullable { get; set; } = true;
}