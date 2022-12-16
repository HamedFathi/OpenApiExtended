// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using OpenApiExtended.Helpers;
using System;
using System.Linq;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static bool IsOpenApiPathItem(this object obj)
    {
        return obj is OpenApiPathItem;
    }

    public static bool IsOpenApiRequestBody(this object obj)
    {
        return obj is OpenApiRequestBody;
    }

    public static bool IsOpenApiResponse(this object obj)
    {
        return obj is OpenApiResponse;
    }

    public static bool IsOpenApiResponses(this object obj)
    {
        return obj is OpenApiResponses;
    }

    public static bool IsOpenApiSchema(this object obj)
    {
        return obj is OpenApiSchema;
    }

    private static object? ConvertToOpenApiMimeType<T>(string mimeType) where T : Enum
    {
        var result = Utilities.GetEnumDetails<T>().FirstOrDefault(x => x.Description == mimeType);
        if (result == null)
        {
            return null;
        }
        return result.Item;
    }

    private static object? GetOpenApiSchemaDefaultValue(this OpenApiSchema? openApiSchema)
    {
        if (openApiSchema == null) return null;
        if (!openApiSchema.IsPrimitive()) return null;

        var info = OpenApiUtility.GetOpenApiValueType(openApiSchema.Type, openApiSchema.Format);
        return info switch
        {
            OpenApiValueType.Boolean => "true",
            OpenApiValueType.Integer => 0,
            OpenApiValueType.Int32 => 1,
            OpenApiValueType.Int64 => 1,
            OpenApiValueType.Number => 1.0,
            OpenApiValueType.Float => 1.0,
            OpenApiValueType.Double => 1.0,
            OpenApiValueType.String => "\"string\"",
            OpenApiValueType.Date => $"\"{DateTime.Now:yyyy-MM-dd}\"",
            OpenApiValueType.DateTime => $"\"{DateTime.Now:yyyy-MM-ddTHH:mm:ssZ}\"",
            OpenApiValueType.Password => "\"P@ssW0rd\"",
            OpenApiValueType.Byte => "\"ZGVmYXVsdA==\"",
            OpenApiValueType.Email => "\"test@example.com\"",
            OpenApiValueType.Uuid => $"\"{Guid.NewGuid()}\"",
            OpenApiValueType.Uri => "\"https://www.example.com/\"",
            OpenApiValueType.HostName => "\"https://www.domain.example.com/\"",
            OpenApiValueType.IPv4 => "\"127.0.0.1\"",
            OpenApiValueType.IPv6 => "\"::1\"",
            OpenApiValueType.Null => null,
            _ => string.Empty
        };
    }
}