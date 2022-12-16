// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended.Models;

public class OpenApiSchemaInfo
{
    private const string DefaultKeySeparator = ".";

    public OpenApiSchemaInfo()
    {
        KeySeparator = DefaultKeySeparator;
        Parents = new List<string>();
    }

    public OpenApiSchemaInfo(string keySeparator)
    {
        KeySeparator = keySeparator;
        Parents = new List<string>();
    }

    public string Key
    {
        get
        {
            if (string.IsNullOrEmpty(KeySeparator))
            {
                KeySeparator = DefaultKeySeparator;
            }
            var parents = Parents.Count > 0 ? Parents.Aggregate((a, b) => a + KeySeparator + b) : string.Empty;
            return parents == string.Empty ? Name : parents + KeySeparator + Name;
        }
    }

    public string KeySeparator { get; private set; }

    public string Name { get; set; } = null!;

    public string ParentKey
    {
        get
        {
            if (string.IsNullOrEmpty(KeySeparator))
            {
                KeySeparator = DefaultKeySeparator;
            }
            return Parents.Count > 0 ? Parents.Aggregate((a, b) => a + KeySeparator + b) : string.Empty;
        }
    }

    public List<string> Parents { get; set; }
    public OpenApiSchema Schema { get; set; } = null!;
    public string Type { get; set; } = null!;
}