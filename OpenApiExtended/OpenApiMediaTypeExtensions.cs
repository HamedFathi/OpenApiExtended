// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using OpenApiExtended.Models;
using System;
using System.Collections.Generic;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static IEnumerable<OpenApiSchemaInfo?> FlattenSchema(this OpenApiMediaType openApiMediaType, string keySeparator = ".",
        string root = "$")
    {
        return openApiMediaType.Schema.Flatten(keySeparator, root);
    }

    public static void Traverse(this OpenApiMediaType openApiMediaType, Action<OpenApiSchemaInfo> action, string keySeparator = ".",
        string root = "$")
    {
        var items = openApiMediaType.FlattenSchema(keySeparator, root);
        foreach (var item in items)
        {
            if (item != null)
            {
                action?.Invoke(item);
            }
        }
    }
}