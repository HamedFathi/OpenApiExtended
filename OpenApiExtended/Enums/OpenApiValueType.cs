// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using System.ComponentModel;

namespace OpenApiExtended.Enums;

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