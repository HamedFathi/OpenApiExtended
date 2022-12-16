// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiRequestBody openApiRequestBody, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
            .Select(x => x.Value).ToList();

        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiRequestBody openApiRequestBody, Func<string, bool> predicate)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;
        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiRequestBody openApiRequestBody, Func<string, bool> predicate, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
            .Where(x => predicate(x.Key))
            .Select(x => x.Value).ToList();

        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiRequestBody openApiRequestBody, Func<OpenApiMimeType, bool> predicate)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;

        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiRequestBody openApiRequestBody, Func<OpenApiMimeType, bool> predicate, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
            .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiRequestBody openApiRequestBody, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiRequestBody openApiRequestBody, Func<OpenApiMimeType, bool> predicate)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiRequestBody openApiRequestBody, Func<OpenApiMimeType, bool> predicate, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiRequestBody openApiRequestBody, Func<string, bool> predicate)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiRequestBody openApiRequestBody, Func<string, bool> predicate, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiRequestBody openApiRequestBody, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
            .Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiRequestBody openApiRequestBody, Func<string, bool> predicate)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiRequestBody openApiRequestBody, Func<string, bool> predicate, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
            .Where(x => predicate(x.Key))
            .Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiRequestBody openApiRequestBody, Func<OpenApiMimeType, bool> predicate)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        var apiMediaTypes = openApiRequestBody.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiRequestBody openApiRequestBody, Func<OpenApiMimeType, bool> predicate, out int count)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }

        var apiMediaTypes = openApiRequestBody.Content
            .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        return apiMediaTypes;
    }

    public static bool HasContent(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        return openApiRequestBody.Content != null && openApiRequestBody.Content.Any();
    }

    public static bool HasNoContent(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        return !openApiRequestBody.HasContent();
    }

    public static bool HasNoReference(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        return !openApiRequestBody.HasReference();
    }

    public static bool HasReference(this OpenApiRequestBody openApiRequestBody)
    {
        if (openApiRequestBody == null)
        {
            throw new ArgumentNullException(nameof(openApiRequestBody));
        }
        return openApiRequestBody.Reference != null;
    }
}