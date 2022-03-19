using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace OpenApiExtended
{
    public static class OpenApiExtensions
    {
        public static OpenApiInfo GetInfo(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Info;
        }
        public static string GetTitle(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Info.Title;
        }
        public static string GetVersion(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Info.Version;
        }
        public static Version GetSemanticVersion(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }

            var versionInfo = openApiDocument.Info.Version;

            try
            {
                var version = Version.Parse(versionInfo);
                return version;
            }
            catch
            {
                return null;
            }
        }
        public static OpenApiLicense GetLicense(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Info.License;
        }
        public static string GetDescription(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Info.Description;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static IList<OpenApiSecurityRequirement> GetSecurityRequirements(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.SecurityRequirements;
        }
        public static IList<OpenApiSecurityRequirement> GetSecurityRequirements(this OpenApiDocument openApiDocument, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var result = openApiDocument.SecurityRequirements;
            count = result.Count;
            return result;
        }
        public static IList<OpenApiSecurityRequirement> GetSecurityRequirements(this OpenApiDocument openApiDocument, Func<OpenApiSecurityRequirement, bool> predicate)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var result = openApiDocument.SecurityRequirements.Where(predicate).ToList();
            return result;
        }
        public static IList<OpenApiSecurityRequirement> GetSecurityRequirements(this OpenApiDocument openApiDocument, Func<OpenApiSecurityRequirement, bool> predicate, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var result = openApiDocument.SecurityRequirements.Where(predicate).ToList();
            count = result.Count;
            return result;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static OpenApiPaths GetPaths(this OpenApiDocument openApiDocument, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var result = openApiDocument.Paths;
            count = result.Count;
            return result;
        }
        public static OpenApiPaths GetPaths(this OpenApiDocument openApiDocument, Func<string, bool> predicate, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var path = new OpenApiPaths();
            var result = openApiDocument.Paths.Where(x => predicate(x.Key));
            foreach (var r in result)
            {
                path.Add(r.Key, r.Value);
            }
            count = path.Count;
            return path;
        }
        public static OpenApiPaths GetPaths(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var result = openApiDocument.Paths;
            return result;
        }
        public static OpenApiPaths GetPaths(this OpenApiDocument openApiDocument, Func<string, bool> predicate)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var path = new OpenApiPaths();
            var result = openApiDocument.Paths.Where(x => predicate(x.Key));
            foreach (var r in result)
            {
                path.Add(r.Key, r.Value);
            }
            return path;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static bool HasParameter(this OpenApiOperation openApiOperation)
        {
            if (openApiOperation == null)
            {
                throw new ArgumentNullException(nameof(openApiOperation));
            }
            return openApiOperation.Parameters != null && openApiOperation.Parameters.Any();
        }
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static bool HasRequestBody(this OpenApiOperation openApiOperation)
        {
            if (openApiOperation == null)
            {
                throw new ArgumentNullException(nameof(openApiOperation));
            }
            return openApiOperation.RequestBody != null;
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
            var requestBody = openApiOperations.Where(x => HasRequestBody(x)).Select(x => x.RequestBody).ToList();
            return requestBody;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static object ConvertToOpenApiMimeType<T>(string mimeType) where T : Enum
        {
            var result = Helper.GetEnumInfo<T>().FirstOrDefault(x => x.Description == mimeType);
            if (result == null)
            {
                return null;
            }
            return (T)Enum.Parse(typeof(T), result.Value, true);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
            count = apiMediaTypes.Count;
            var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
            return schemas;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static bool HasResponse(this OpenApiOperation openApiOperation)
        {
            if (openApiOperation == null)
            {
                throw new ArgumentNullException(nameof(openApiOperation));
            }
            return openApiOperation.Responses != null && openApiOperation.Responses.Any();
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static IList<string> GetPathsKeys(this OpenApiDocument openApiDocument)
        {
            var list = new List<string>();
            var paths = GetPaths(openApiDocument);
            foreach (var path in paths)
            {
                var pathKey = path.Key;
                list.Add(pathKey);
            }
            return list;
        }
        public static IList<string> GetOperationsKeys(this OpenApiDocument openApiDocument)
        {
            var list = new List<string>();
            var paths = GetPaths(openApiDocument);
            foreach (var path in paths)
            {
                var pathKey = path.Key;
                var operations = path.Value;
                foreach (var operation in operations.Operations)
                {
                    var operationKey = operation.Key.ToString().ToLower();
                    list.Add($"{pathKey}>{operationKey}");
                }
            }

            return list;
        }
        public static IList<string> GetResponsesKeys(this OpenApiDocument openApiDocument)
        {
            var list = new List<string>();
            var paths = GetPaths(openApiDocument);
            foreach (var path in paths)
            {
                var pathKey = path.Key;
                var operations = path.Value;
                foreach (var operation in operations.Operations)
                {
                    var operationKey = operation.Key.ToString().ToLower();
                    foreach (var response in operation.Value.Responses)
                    {
                        var responseKey = response.Key;
                        list.Add($"{pathKey}>{operationKey}>{responseKey}");
                    }
                }
            }
            return list;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument)
        {
            return openApiDocument.Components.Schemas.Select(x => x.Value).ToList();
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, out int count)
        {
            var schemas = openApiDocument.Components.Schemas.Select(x => x.Value).ToList();
            count = schemas.Count;
            return schemas;
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<string, bool> schemaId)
        {
            return openApiDocument.Components.Schemas.Where(x => schemaId(x.Key)).Select(x => x.Value).ToList();
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<string, bool> schemaId, out int count)
        {
            var schemas = openApiDocument.Components.Schemas.Where(x => schemaId(x.Key)).Select(x => x.Value).ToList();
            count = schemas.Count;
            return schemas;
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<OpenApiSchema, bool> schema)
        {
            var schemas = openApiDocument.Components.Schemas.Select(x => x.Value);
            var result = schemas.Where(x => schema(x)).ToList();
            return result;
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<OpenApiSchema, bool> schema, out int count)
        {
            var schemas = openApiDocument.Components.Schemas.Select(x => x.Value);
            var result = schemas.Where(x => schema(x)).ToList();
            count = result.Count;
            return result;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static string ConvertToJsonExample(this OpenApiSchema openApiSchema)
        {


            return null;
        }
    }
}
