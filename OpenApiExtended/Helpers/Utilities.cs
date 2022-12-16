// ReSharper disable UnusedMember.Global
using OpenApiExtended.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended.Helpers;

internal static class Utilities
{
    public static IEnumerable<EnumDetail<T>> GetEnumDetails<T>() where T : Enum
    {
        var result = new List<EnumDetail<T>>();
        var names = Enum.GetNames(typeof(T));
        foreach (var name in names)
        {
            var parsed = Enum.Parse(typeof(T), name);
            var item = (T)parsed;
            var value = Convert.ToInt32(parsed);
            var description = item.GetDescription();
            result.Add(new EnumDetail<T>
            {
                Name = name,
                Value = value,
                Description = description,
                Item = item
            });
        }

        return result;
    }

    public static IEnumerable<EnumDetail<T>> GetEnumDetails<T>(Func<EnumDetail<T>, bool> predicate) where T : Enum
    {
        return GetEnumDetails<T>().Where(predicate);
    }
}