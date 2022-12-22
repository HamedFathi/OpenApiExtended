using OpenApiExtended.Enums;

namespace OpenApiExtended.Models;

public class TypeScriptSourceSettings
{
    public string InterfaceOrTypeName { get; set; } = "Root";
    public bool UseAny { get; set; } = false;
    public bool UseDate { get; set; } = true;
    public TypeScriptResultKind ResultKind { get; set; } = TypeScriptResultKind.Interface;
}