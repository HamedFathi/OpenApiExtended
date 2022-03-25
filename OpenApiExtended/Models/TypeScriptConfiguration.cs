namespace OpenApiExtended;

public class TypeScriptConfiguration
{
    public bool StartInterfaceNameWithI { get; set; } = false;
    public bool ReplaceEmptyObjectsWithUnknownType { get; set; } = false;
    public bool GenerateComments { get; set; } = false;
    public bool ExportWithDefault { get; set; } = false;
}