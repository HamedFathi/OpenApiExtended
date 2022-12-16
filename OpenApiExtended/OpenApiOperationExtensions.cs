// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static IList<OpenApiParameter> GetParameters(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var parameters = openApiOperation.Parameters;
        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var parameters = openApiOperation.Parameters;
        count = parameters.Count;
        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this OpenApiOperation openApiOperation, Func<OpenApiParameter, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var parameters = openApiOperation.Parameters;

        if (predicate != null)
        {
            parameters = parameters.Where(predicate).ToList();
        }

        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this OpenApiOperation openApiOperation, Func<OpenApiParameter, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var parameters = openApiOperation.Parameters;

        if (predicate != null)
        {
            parameters = parameters.Where(predicate).ToList();
        }

        count = parameters.Count;
        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this IEnumerable<OpenApiOperation> openApiOperations)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }
        var parameters = openApiOperations.Where(x => x.HasParameter()).SelectMany(x => x.Parameters).ToList();
        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this IEnumerable<OpenApiOperation> openApiOperations, out int count)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }
        var parameters = openApiOperations.Where(x => x.HasParameter()).SelectMany(x => x.Parameters).ToList();
        count = parameters.Count;
        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this IEnumerable<OpenApiOperation> openApiOperations, Func<OpenApiParameter, bool> predicate)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }

        var parameters = openApiOperations.Where(x => x.HasParameter()).SelectMany(x => x.Parameters).ToList();

        if (predicate != null)
        {
            parameters = parameters.Where(predicate).ToList();
        }

        return parameters;
    }

    public static IList<OpenApiParameter> GetParameters(this IEnumerable<OpenApiOperation> openApiOperations, Func<OpenApiParameter, bool> predicate, out int count)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }
        var parameters = openApiOperations.Where(x => x.HasParameter()).SelectMany(x => x.Parameters).ToList();
        if (predicate != null)
        {
            parameters = parameters.Where(predicate).ToList();
        }
        count = parameters.Count;
        return parameters;
    }

    public static OpenApiRequestBody GetRequestBody(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        return requestBody;
    }

    public static IList<OpenApiRequestBody> GetRequestBody(this IEnumerable<OpenApiOperation> openApiOperations)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }
        var requestBody = openApiOperations.Where(HasRequestBody).Select(x => x.RequestBody).ToList();
        return requestBody;
    }

    public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var requestBody = openApiOperation.RequestBody;
        var apiMediaTypes = requestBody?.Content.Select(x => x.Value).ToList();
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }
        var apiMediaTypes = requestBody.Content.Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, Func<string, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            return null;
        }

        var apiMediaTypes = requestBody.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, Func<string, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }

        var apiMediaTypes = requestBody.Content
            .Where(x => predicate(x.Key))
            .Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, Func<OpenApiMimeType, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            return null;
        }

        var apiMediaTypes = requestBody.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;
        return apiMediaTypes;
    }

    public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, Func<OpenApiMimeType, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }

        var apiMediaTypes = requestBody.Content
            .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value).ToList();
        count = apiMediaTypes.Count;
        return apiMediaTypes;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            return null;
        }
        var apiMediaTypes = requestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }
        var apiMediaTypes = requestBody.Content
            .Select(x => x.Value).ToList();

        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, Func<string, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            return null;
        }
        var apiMediaTypes = requestBody.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;
        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, Func<string, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }
        var apiMediaTypes = requestBody.Content
            .Where(x => predicate(x.Key))
            .Select(x => x.Value).ToList();

        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, Func<OpenApiMimeType, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;

        var apiMediaTypes = requestBody?.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;

        var schemas = apiMediaTypes?.Select(x => x.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, Func<OpenApiMimeType, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }
        var apiMediaTypes = requestBody.Content
            .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
            .Select(x => x.Value).ToList();
        var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            return null;
        }
        var apiMediaTypes = requestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;
        if (requestBody == null)
        {
            count = 0;
            return null;
        }
        var apiMediaTypes = requestBody.Content
                .Select(x => x.Value)
                .ToList()
            ;
        var refs = apiMediaTypes.Select(x => x.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiOperation openApiOperation, Func<OpenApiMimeType, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;

        var apiMediaTypes = requestBody?.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;

        var refs = apiMediaTypes?.Select(x => x.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiOperation openApiOperation, Func<OpenApiMimeType, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;

        var apiMediaTypes = requestBody?.Content
                .Where(x => predicate((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
            ;

        var refs = apiMediaTypes?.Select(x => x.Schema.Reference).ToList();
        count = refs?.Count ?? 0;
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiOperation openApiOperation, Func<string, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;

        var apiMediaTypes = requestBody?.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;

        var refs = apiMediaTypes?.Select(x => x.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetRequestBodySchemaReference(this OpenApiOperation openApiOperation, Func<string, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var requestBody = openApiOperation.RequestBody;

        var apiMediaTypes = requestBody?.Content
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToList()
            ;

        var refs = apiMediaTypes?.Select(x => x.Schema.Reference).ToList();
        count = refs?.Count ?? 0;
        return refs;
    }

    public static OpenApiResponses GetResponses(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var responses = openApiOperation.Responses;
        return responses;
    }

    public static OpenApiResponses GetResponses(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var responses = openApiOperation.Responses;
        count = responses.Count;
        return responses;
    }

    public static IList<OpenApiResponse> GetResponses(this IEnumerable<OpenApiOperation> openApiOperations)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }

        var responses = openApiOperations.Where(x => x.HasResponse()).SelectMany(x => x.Responses).Select(x => x.Value).ToList();
        return responses;
    }

    public static IList<OpenApiResponse> GetResponses(this IEnumerable<OpenApiOperation> openApiOperations, out int count)
    {
        if (openApiOperations == null)
        {
            throw new ArgumentNullException(nameof(openApiOperations));
        }

        var responses = openApiOperations.Where(x => x.HasResponse()).SelectMany(x => x.Responses).Select(x => x.Value).ToList();
        count = responses.Count;
        return responses;
    }

    public static IList<OpenApiResponse> GetResponses(this OpenApiOperation openApiOperation, Func<string, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = openApiOperation.Responses.Where(x => predicate(x.Key)).Select(x => x.Value).ToList();
        return responses;
    }

    public static IList<OpenApiResponse> GetResponses(this OpenApiOperation openApiOperation, Func<string, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = openApiOperation.Responses.Where(x => predicate(x.Key)).Select(x => x.Value).ToList();
        count = responses.Count;
        return responses;
    }

    public static IList<OpenApiResponse> GetResponses(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> predicate)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = openApiOperation.Responses
            .Where(x => predicate((HttpResponseStatusCode)Enum.Parse(typeof(HttpResponseStatusCode), x.Key)))
            .Select(x => x.Value).ToList();
        return responses;
    }

    public static IList<OpenApiResponse> GetResponses(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> predicate, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = openApiOperation.Responses
            .Where(x => predicate((HttpResponseStatusCode)Enum.Parse(typeof(HttpResponseStatusCode), x.Key)))
            .Select(x => x.Value).ToList();
        count = responses.Count;
        return responses;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation);
        var content = responses.Select(x => x.Value).SelectMany(x => x.Content).Select(x => x.Value).ToList();
        return content;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation);
        var content = responses.Select(x => x.Value).SelectMany(x => x.Content).Select(x => x.Value).ToList();
        count = content.Count;
        return content;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiOperation openApiOperation, Func<string, bool> httpResponseStatusCode, Func<string, bool> mimeType)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var content = responses.SelectMany(x => x.Content).Where(x => mimeType(x.Key)).Select(x => x.Value).ToList();
        return content;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiOperation openApiOperation, Func<string, bool> httpResponseStatusCode, Func<string, bool> mimeType, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var content = responses.SelectMany(x => x.Content).Where(x => mimeType(x.Key)).Select(x => x.Value).ToList();
        count = content.Count;
        return content;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> httpResponseStatusCode, Func<OpenApiMimeType, bool> mimeType)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var content = responses
            .SelectMany(x => x.Content)
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key))).Select(x => x.Value).ToList();
        return content;
    }

    public static IList<OpenApiMediaType> GetResponsesContent(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> httpResponseStatusCode, Func<OpenApiMimeType, bool> mimeType, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var content = responses.SelectMany(x => x.Content)
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key))).Select(x => x.Value).ToList();
        count = content.Count;
        return content;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation);
        var schemas = responses.Select(x => x.Value).SelectMany(x => x.Content).Select(x => x.Value.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation);
        var schemas = responses.Select(x => x.Value).SelectMany(x => x.Content).Select(x => x.Value.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiOperation openApiOperation, Func<string, bool> httpResponseStatusCode, Func<string, bool> mimeType)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }

        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var schemas = responses.SelectMany(x => x.Content).Where(x => mimeType(x.Key)).Select(x => x.Value.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiOperation openApiOperation, Func<string, bool> httpResponseStatusCode, Func<string, bool> mimeType, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var schemas = responses.SelectMany(x => x.Content).Where(x => mimeType(x.Key)).Select(x => x.Value.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> httpResponseStatusCode, Func<OpenApiMimeType, bool> mimeType)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var schemas = responses
            .SelectMany(x => x.Content)
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key))).Select(x => x.Value.Schema).ToList();
        return schemas;
    }

    public static IList<OpenApiSchema> GetResponsesSchema(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> httpResponseStatusCode, Func<OpenApiMimeType, bool> mimeType, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var schemas = responses.SelectMany(x => x.Content)
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key))).Select(x => x.Value.Schema).ToList();
        count = schemas.Count;
        return schemas;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation);
        var refs = responses.Select(x => x.Value).SelectMany(x => x.Content).Select(x => x.Value.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiOperation openApiOperation, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation);
        var refs = responses.Select(x => x.Value).SelectMany(x => x.Content).Select(x => x.Value.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> httpResponseStatusCode, Func<OpenApiMimeType, bool> mimeType)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var refs = responses
            .SelectMany(x => x.Content)
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key))).Select(x => x.Value.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiOperation openApiOperation, Func<HttpResponseStatusCode, bool> httpResponseStatusCode, Func<OpenApiMimeType, bool> mimeType, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var refs = responses.SelectMany(x => x.Content)
            .Where(x => mimeType((OpenApiMimeType)ConvertToOpenApiMimeType<OpenApiMimeType>(x.Key))).Select(x => x.Value.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiOperation openApiOperation, Func<string, bool> httpResponseStatusCode, Func<string, bool> mimeType)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var refs = responses
            .SelectMany(x => x.Content)
            .Where(x => mimeType(x.Key)).Select(x => x.Value.Schema.Reference).ToList();
        return refs;
    }

    public static IList<OpenApiReference> GetResponsesSchemaReference(this OpenApiOperation openApiOperation, Func<string, bool> httpResponseStatusCode, Func<string, bool> mimeType, out int count)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        var responses = GetResponses(openApiOperation, httpResponseStatusCode);
        var refs = responses.SelectMany(x => x.Content)
            .Where(x => mimeType(x.Key)).Select(x => x.Value.Schema.Reference).ToList();
        count = refs.Count;
        return refs;
    }

    public static bool HasParameter(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        return openApiOperation.Parameters != null && openApiOperation.Parameters.Any();
    }

    public static bool HasRequestBody(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        return openApiOperation.RequestBody != null;
    }

    public static bool HasResponse(this OpenApiOperation openApiOperation)
    {
        if (openApiOperation == null)
        {
            throw new ArgumentNullException(nameof(openApiOperation));
        }
        return openApiOperation.Responses != null && openApiOperation.Responses.Any();
    }
}