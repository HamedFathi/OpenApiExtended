namespace OpenApiExtended;

public class TypeScriptMemberInfo
{
    public string Name { get; set; }
    public TypeScriptMemberInfoType Type { get; set; }
    public bool IsOptional { get; set; }
    public bool IsNullable { get; set; }
}