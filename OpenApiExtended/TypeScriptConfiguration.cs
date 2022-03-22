using OpenApiExtended.Enums;

namespace OpenApiExtended;

public class TypeScriptConfiguration
{
    public TypeScriptResult TypeScriptResult { get; set; } = TypeScriptResult.Interface;
    public bool StartInterfaceNameWithI { get; set; } = false;
    public bool ReplaceEmptyObjectsWithUnknownType { get; set; } = false;
    public bool GenerateComments { get; set; } = false;
    public bool ExportWithDefault { get; set; } = false;
    public bool UseGuidType { get; set; } = false;
}