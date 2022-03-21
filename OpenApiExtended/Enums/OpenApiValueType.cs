using System.ComponentModel;

namespace OpenApiExtended
{
    public enum OpenApiValueType
    {
        Unknown,
        Boolean,
        Integer,
        Int32,
        Int64,
        Number,
        Float,
        Double,
        String,
        Date,
        [Description("date-time")]
        DateTime,
        Password,
        Byte,
        Binary,
        Email,
        Uuid,
        Uri,
        HostName,
        IPv4,
        IPv6,
        Null,
        Array,
        Object
    }
}
