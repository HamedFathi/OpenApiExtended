// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static IList<OpenApiOperation> GetOperations(this OpenApiPaths pathItems)
    {
        if (pathItems == null)
        {
            throw new ArgumentNullException(nameof(pathItems));
        }

        var operations = pathItems.Values.SelectMany(x => x.Operations);
        var result = operations.Select(x => x.Value).ToList();
        return result;
    }

    public static IList<OpenApiOperation> GetOperations(this OpenApiPaths pathItems, Func<OperationType, bool> predicate)
    {
        if (pathItems == null)
        {
            throw new ArgumentNullException(nameof(pathItems));
        }
        var operations = pathItems.Values
                .SelectMany(x => x.Operations)
                .Where(x => predicate(x.Key))
            ;

        var result = operations.Select(x => x.Value).ToList();
        return result;
    }

    public static IList<OpenApiOperation> GetOperations(this OpenApiPaths pathItems, out int count)
    {
        if (pathItems == null)
        {
            throw new ArgumentNullException(nameof(pathItems));
        }

        var operations = pathItems.Values.SelectMany(x => x.Operations).ToList();
        var result = operations.Select(x => x.Value).ToList();

        count = result.Count;
        return result;
    }

    public static IList<OpenApiOperation> GetOperations(this OpenApiPaths pathItems, Func<OperationType, bool> predicate, out int count)
    {
        if (pathItems == null)
        {
            throw new ArgumentNullException(nameof(pathItems));
        }

        var operations = pathItems.Values
            .SelectMany(x => x.Operations)
            .Where(x => predicate(x.Key))
            .ToList();

        var result = operations.Select(x => x.Value).ToList();

        count = result.Count;
        return result;
    }
}