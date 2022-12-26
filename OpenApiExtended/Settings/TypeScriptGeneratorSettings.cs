using OpenApiExtended.Enums;

namespace OpenApiExtended.Settings;

public class TypeScriptGeneratorSettings
{
    public string InterfaceOrTypeName { get; set; } = "Root";
    public TypeScriptTypeStyle TypeStyle { get; set; } = TypeScriptTypeStyle.Interface;
    public bool UseAny { get; set; } = false;
    public bool UseDate { get; set; } = true;
}