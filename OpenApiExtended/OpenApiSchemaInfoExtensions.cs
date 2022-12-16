// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

using OpenApiExtended.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static string? GetParentType(this OpenApiSchemaInfo? openApiSchemaInfo, IEnumerable<OpenApiSchemaInfo?>? openApiSchemaInfos)
    {
        if (openApiSchemaInfos == null) return null;
        if (openApiSchemaInfo == null) return null;

        return openApiSchemaInfos.LastOrDefault(x => x?.Key == openApiSchemaInfo.ParentKey)?.Type;
    }

    public static string? GetParentType(this IEnumerable<OpenApiSchemaInfo?>? openApiSchemaInfos, OpenApiSchemaInfo? openApiSchemaInfo)
    {
        if (openApiSchemaInfos == null) return null;
        if (openApiSchemaInfo == null) return null;
        return openApiSchemaInfo.GetParentType(openApiSchemaInfos);
    }

    public static string? GetRoot(this IEnumerable<OpenApiSchemaInfo?>? openApiSchemaInfos)
    {
        if (openApiSchemaInfos == null) return null;

        var apiSchemaInfos = openApiSchemaInfos.ToList();
        var root = apiSchemaInfos.SingleOrDefault(x => x?.Parents.Count == 0);
        return root?.Name;
    }

    public static string? GetRoot(this OpenApiSchemaInfo? openApiSchemaInfo)
    {
        if (openApiSchemaInfo == null) return null;
        var root = openApiSchemaInfo.Key.Split(new[] { openApiSchemaInfo.KeySeparator }, StringSplitOptions.None).First();
        return root;
    }

    public static bool IsArrayItem(this OpenApiSchemaInfo? openApiSchemaInfo, IEnumerable<OpenApiSchemaInfo?>? openApiSchemaInfos)
    {
        if (openApiSchemaInfos == null) return false;
        if (openApiSchemaInfo == null) return false;

        var parent = openApiSchemaInfos.LastOrDefault(x => x?.Key == openApiSchemaInfo.ParentKey)?.Type;
        if (parent == null) return false;

        return openApiSchemaInfo.Schema.IsPrimitive() && parent == "array";
    }

    public static bool IsArrayItem(this IEnumerable<OpenApiSchemaInfo?>? openApiSchemaInfos, OpenApiSchemaInfo? openApiSchemaInfo)
    {
        if (openApiSchemaInfos == null) return false;
        if (openApiSchemaInfo == null) return false;
        return openApiSchemaInfo.IsArrayItem(openApiSchemaInfos);
    }

    public static bool IsRoot(this OpenApiSchemaInfo? openApiSchemaInfo)
    {
        if (openApiSchemaInfo == null) return false;
        return openApiSchemaInfo.Parents.Count == 0;
    }
}