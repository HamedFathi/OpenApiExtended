using OpenApiExtended.Enums;

namespace OpenApiExtended.Models;

public class CSharpSourceSettings
{
    public string Namespace { get; set; } = "RootNameSpace";
    public string ClassOrRecordName { get; set; } = "Root";
    public bool FileScopedNamespace { get; set; } = false;
    public bool UseArray { get; set; } = false;
    public bool UseDateTime { get; set; } = true;
    public bool UseNullable { get; set; } = true;
    public CSharpResultKind ResultKind { get; set; } = CSharpResultKind.Class;
}