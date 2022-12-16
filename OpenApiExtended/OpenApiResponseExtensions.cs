// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var mediaTypes = openApiResponse.Content.Select(x => x.Value).ToList();
        return mediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiResponse openApiResponse, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var mediaTypes = openApiResponse.Content.Select(x => x.Value).ToList();
        count = mediaTypes.Count;
        return mediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiResponse openApiResponse, Func<string, bool> mimeType)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }

        var mediaTypes = openApiResponse.Content
            .Where(x => mimeType(x.Key))
            .Select(x => x.Value)
            .ToList();
        return mediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiResponse openApiResponse, Func<string, bool> mimeType, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var mediaTypes = openApiResponse.Content
            .Where(x => mimeType(x.Key))
            .Select(x => x.Value)
            .ToList();
        count = mediaTypes.Count;
        return mediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiResponse openApiResponse, Func<OpenApiMimeType, bool> mimeType)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }

        var mediaTypes = openApiResponse.Content
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value)
            .ToList();
        return mediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiResponse openApiResponse, Func<OpenApiMimeType, bool> mimeType, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var mediaTypes = openApiResponse.Content
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value)
            .ToList();
        count = mediaTypes.Count;
        return mediaTypes;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var schemas = openApiResponse.Content.Select(x => x.Value.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiResponse openApiResponse, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var schemas = openApiResponse.Content.Select(x => x.Value.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiResponse openApiResponse, Func<string, bool> mimeType)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }

        var schemas = openApiResponse.Content
            .Where(x => mimeType(x.Key))
            .Select(x => x.Value.Schema)
            .ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiResponse openApiResponse, Func<string, bool> mimeType, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var schemas = openApiResponse.Content
            .Where(x => mimeType(x.Key))
            .Select(x => x.Value.Schema)
            .ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiResponse openApiResponse, Func<OpenApiMimeType, bool> mimeType)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }

        var schemas = openApiResponse.Content
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value.Schema)
            .ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiResponse openApiResponse, Func<OpenApiMimeType, bool> mimeType, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var schemas = openApiResponse.Content
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value.Schema)
            .ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var refs = openApiResponse.Content.Select(x => x.Value.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiResponse openApiResponse, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var refs = openApiResponse.Content.Select(x => x.Value.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiResponse openApiResponse, Func<OpenApiMimeType, bool> mimeType)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var refs = openApiResponse.Content
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value.Schema.Reference)
            .ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiResponse openApiResponse, Func<OpenApiMimeType, bool> mimeType, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var refs = openApiResponse.Content
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value.Schema.Reference)
            .ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiResponse openApiResponse, Func<string, bool> mimeType)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var refs = openApiResponse.Content
            .Where(x => mimeType(x.Key))
            .Select(x => x.Value.Schema.Reference)
            .ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiResponse openApiResponse, Func<string, bool> mimeType, out int count)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        var refs = openApiResponse.Content
            .Where(x => mimeType(x.Key))
            .Select(x => x.Value.Schema.Reference)
            .ToList();
        count = refs.Count;
        return refs;
    }

    public static bool HasContent(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        return openApiResponse.Content != null && openApiResponse.Content.Any();
    }

    public static bool HasNoContent(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        return !openApiResponse.HasContent();
    }

    public static bool HasNoReference(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        return !openApiResponse.HasReference();
    }

    public static bool HasReference(this OpenApiResponse openApiResponse)
    {
        if (openApiResponse == null)
        {
            throw new ArgumentNullException(nameof(openApiResponse));
        }
        return openApiResponse.Reference != null;
    }
}