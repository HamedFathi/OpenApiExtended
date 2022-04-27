using System;

namespace OpenApiExtended;

public class EnumMemberInfo<T> where T : Enum
{
    public string Name { get; set; }
    public T Item { get; set; }
    public int Value { get; set; }
    public string Description { get; set; }
}