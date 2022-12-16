// ReSharper disable UnusedMember.Global
using OpenApiExtended.Enums;
using System;

namespace OpenApiExtended;

public static class OpenApiUtility
{
    public static string GetCSharpType(OpenApiValueType openApiValueType, string typeOfCollection = "object", bool useArray = false, bool useDateTimeType = true)
    {
        return openApiValueType switch
        {
            OpenApiValueType.Unknown => "object",
            OpenApiValueType.Binary => "object",
            OpenApiValueType.Boolean => "bool",
            OpenApiValueType.Integer => "int",
            OpenApiValueType.Int32 => "int",
            OpenApiValueType.Int64 => "long",
            OpenApiValueType.Number => "double",
            OpenApiValueType.Float => "float",
            OpenApiValueType.Double => "double",
            OpenApiValueType.Date => useDateTimeType ? "DateTime" : "string",
            OpenApiValueType.DateTime => useDateTimeType ? "DateTime" : "string",
            OpenApiValueType.String => "string",
            OpenApiValueType.Password => "string",
            OpenApiValueType.Byte => "string",
            OpenApiValueType.Email => "string",
            OpenApiValueType.Uuid => "string",
            OpenApiValueType.Uri => "string",
            OpenApiValueType.HostName => "string",
            OpenApiValueType.IPv4 => "string",
            OpenApiValueType.IPv6 => "string",
            OpenApiValueType.Null => "object",
            OpenApiValueType.Array => useArray ? $"{typeOfCollection}[]" : $"List<{typeOfCollection}>",
            OpenApiValueType.Object => "object",
            _ => throw new ArgumentOutOfRangeException(nameof(openApiValueType), openApiValueType, null)
        };
    }

    public static OpenApiValueType GetOpenApiValueType(string type, string format)
    {
        if (!string.IsNullOrEmpty(format))
        {
            if (string.Equals(format, "int32", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Int32;
            if (string.Equals(format, "int64", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Int64;
            if (string.Equals(format, "float", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Float;
            if (string.Equals(format, "double", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Double;
            if (string.Equals(format, "date", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Date;
            if (string.Equals(format, "date-time", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.DateTime;
            if (string.Equals(format, "password", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Password;
            if (string.Equals(format, "byte", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Byte;
            if (string.Equals(format, "binary", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Binary;
            if (string.Equals(format, "email", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Email;
            if (string.Equals(format, "uuid", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Uuid;
            if (string.Equals(format, "uri", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Uri;
            if (string.Equals(format, "hostname", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.HostName;
            if (string.Equals(format, "ipv4", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.IPv4;
            if (string.Equals(format, "ipv6", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.IPv6;
        }

        if (!string.IsNullOrEmpty(type))
        {
            if (string.Equals(type, "number", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Number;
            if (string.Equals(type, "integer", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Integer;
            if (string.Equals(type, "string", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.String;
            if (string.Equals(type, "boolean", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Boolean;
            if (string.Equals(type, "null", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Null;
            if (string.Equals(type, "array", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Array;
            if (string.Equals(type, "object", StringComparison.OrdinalIgnoreCase)) return OpenApiValueType.Object;
        }

        return OpenApiValueType.Unknown;
    }

    public static string GetTypeScriptType(OpenApiValueType openApiValueType, bool useAnyType = false, bool useDateType = true)
    {
        return openApiValueType switch
        {
            OpenApiValueType.Unknown => "unknown",
            OpenApiValueType.Binary => useAnyType ? "any" : "unknown",
            OpenApiValueType.Boolean => "boolean",
            OpenApiValueType.Integer => "number",
            OpenApiValueType.Int32 => "number",
            OpenApiValueType.Int64 => "number",
            OpenApiValueType.Number => "number",
            OpenApiValueType.Float => "number",
            OpenApiValueType.Double => "number",
            OpenApiValueType.Date => useDateType ? "Date" : "string",
            OpenApiValueType.DateTime => useDateType ? "Date" : "string",
            OpenApiValueType.String => "string",
            OpenApiValueType.Password => "string",
            OpenApiValueType.Byte => "string",
            OpenApiValueType.Email => "string",
            OpenApiValueType.Uuid => "string",
            OpenApiValueType.Uri => "string",
            OpenApiValueType.HostName => "string",
            OpenApiValueType.IPv4 => "string",
            OpenApiValueType.IPv6 => "string",
            OpenApiValueType.Null => "null",
            OpenApiValueType.Array => useAnyType ? "any[]" : "unknown[]",
            OpenApiValueType.Object => "object",
            _ => throw new ArgumentOutOfRangeException(nameof(openApiValueType), openApiValueType, null)
        };
    }
}