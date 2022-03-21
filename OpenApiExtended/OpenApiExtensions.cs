﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
// ReSharper disable UnusedMember.Global

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
            var requestBody = openApiOperations.Where(HasRequestBody).Select(x => x.RequestBody).ToList();
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
            var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
            count = schemas.Count;
            return schemas;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                .Where(x => mimeType((OpenApiMimeType)Enum.Parse(typeof(OpenApiMimeType), x.Key)))
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
                .Where(x => mimeType((OpenApiMimeType)Enum.Parse(typeof(OpenApiMimeType), x.Key)))
                .Select(x => x.Value)
                .ToList();
            count = mediaTypes.Count;
            return mediaTypes;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var list = new List<string>();
            var paths = GetPaths(openApiDocument);
            foreach (var path in paths)
            {
                var pathKey = path.Key;
                list.Add(pathKey);
            }
            return list;
        }
        public static IList<string> GetOperationsKeys(this OpenApiDocument openApiDocument, string separator = ">")
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            separator = string.IsNullOrEmpty(separator) ? ">" : separator;
            var list = new List<string>();
            var paths = GetPaths(openApiDocument);
            foreach (var path in paths)
            {
                var pathKey = path.Key;
                var operations = path.Value;
                foreach (var operation in operations.Operations)
                {
                    var operationKey = operation.Key.ToString().ToLower();
                    list.Add($"{pathKey}{separator}{operationKey}");
                }
            }
            return list;
        }
        public static IList<string> GetResponsesKeys(this OpenApiDocument openApiDocument, string separator = ">")
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentNullException(nameof(separator));
            }
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
                        list.Add($"{pathKey}{separator}{operationKey}{separator}{responseKey}");
                    }
                }
            }
            return list;
        }
        public static object FindByKey(this OpenApiDocument openApiDocument, string key, string separator = ">")
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentNullException(nameof(separator));
            }
            var parts = key.Split(new[] { separator }, StringSplitOptions.None);
            var count = parts.Length;
            OpenApiPathItem path = null;
            OpenApiOperation operation = null;
            OpenApiResponse response = null;
            if (count >= 1)
            {
                path = openApiDocument.GetPaths(x => x == parts[0]).FirstOrDefault().Value;
            }
            if (count >= 2 && path != null)
            {
                operation = path.Operations.FirstOrDefault(x => x.Key.ToString().ToLower() == parts[1].ToLower()).Value;
            }
            if (count == 3 && path != null && operation != null)
            {
                response = operation.Responses.FirstOrDefault(x => x.Key == parts[2]).Value;
            }
            return count == 1 ? path : (count == 2 ? operation : (count == 3 ? response : null));
        }
        public static object FindByKey(this OpenApiDocument openApiDocument, string key, out OpenApiKeyType openApiKeyType, string separator = ">")
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentNullException(nameof(separator));
            }
            var parts = key.Split(new[] { separator }, StringSplitOptions.None);
            var count = parts.Length;
            OpenApiPathItem path = null;
            OpenApiOperation operation = null;
            OpenApiResponse response = null;
            openApiKeyType = OpenApiKeyType.None;
            if (count >= 1)
            {
                path = openApiDocument.GetPaths(x => x == parts[0]).FirstOrDefault().Value;
                if (path != null)
                {
                    openApiKeyType = OpenApiKeyType.Path;
                }
            }
            if (count >= 2 && path != null)
            {
                operation = path.Operations.FirstOrDefault(x => x.Key.ToString().ToLower() == parts[1].ToLower()).Value;
                if (operation != null)
                {
                    openApiKeyType = OpenApiKeyType.Operation;
                }
            }
            if (count == 3 && path != null && operation != null)
            {

                response = operation.Responses.FirstOrDefault(x => x.Key == parts[2]).Value;
                if (response != null)
                {
                    openApiKeyType = OpenApiKeyType.Response;
                }
            }

            return count == 1 ? path : (count == 2 ? operation : (count == 3 ? response : null));
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Components.Schemas.Select(x => x.Value).ToList();
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var schemas = openApiDocument.Components.Schemas.Select(x => x.Value).ToList();
            count = schemas.Count;
            return schemas;
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<string, bool> schemaId)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            return openApiDocument.Components.Schemas.Where(x => schemaId(x.Key)).Select(x => x.Value).ToList();
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<string, bool> schemaId, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var schemas = openApiDocument.Components.Schemas.Where(x => schemaId(x.Key)).Select(x => x.Value).ToList();
            count = schemas.Count;
            return schemas;
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<OpenApiSchema, bool> schema)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var schemas = openApiDocument.Components.Schemas.Select(x => x.Value);
            var result = schemas.Where(schema).ToList();
            return result;
        }
        public static IList<OpenApiSchema> GetComponentsSchema(this OpenApiDocument openApiDocument, Func<OpenApiSchema, bool> schema, out int count)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            var schemas = openApiDocument.Components.Schemas.Select(x => x.Value);
            var result = schemas.Where(schema).ToList();
            count = result.Count;
            return result;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static bool IsArray(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Type == "array";
        }
        public static bool IsObject(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Type == "object";
        }
        public static bool IsPrimitive(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return !openApiSchema.IsArray() && !openApiSchema.IsObject();
        }
        public static string GetType(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Type;
        }
        public static string GetFormat(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Format;
        }
        public static bool HasReference(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Reference != null;
        }
        public static bool HasEmptyReference(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Reference != null
                && (openApiSchema.Properties == null || openApiSchema.Properties.Count == 0)
                && (openApiSchema.Items == null)
                ;
        }
        public static OpenApiSchema GetReference(this OpenApiSchema openApiSchema, OpenApiDocument openApiDocument)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }

            if (!openApiSchema.HasReference()) return null;

            var refId = openApiSchema.Reference.Id;
            var result = openApiDocument.GetComponentsSchema(x => x == refId)?.FirstOrDefault();
            return result;
        }
        public static OpenApiSchema GetReference(this OpenApiSchema openApiSchema, OpenApiDocument openApiDocument, out string referenceId)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }

            if (!openApiSchema.HasReference())
            {
                referenceId = null;
                return null;
            }

            var refId = openApiSchema.Reference.Id;
            referenceId = refId;
            var result = openApiDocument.GetComponentsSchema(x => x == refId)?.FirstOrDefault();
            return result;
        }
        public static IEnumerable<string> GetRequired(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Required.Select(x => x);
        }
        public static IEnumerable<string> GetRequired(this OpenApiSchema openApiSchema, Func<string, bool> predicate)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Required.Where(predicate).Select(x => x);
        }
        public static bool IsRequired(this OpenApiSchema openApiSchema, Func<string, bool> predicate)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return openApiSchema.Required.Any(predicate);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static IDictionary<IList<string>, OpenApiSchema> GetSchemaMembers(this OpenApiSchema openApiSchema, IList<string> path = null, string parentType = null, IDictionary<IList<string>, OpenApiSchema> list = null, bool includeMainSchema = false)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            path ??= new List<string>();
            list ??= new Dictionary<IList<string>, OpenApiSchema>();

            if (path.Count == 0 && parentType == null && list.Count == 0 && includeMainSchema)
            {
                var newPath = new List<string>();
                list.Add(newPath, openApiSchema);
            }
            // Array Type
            if (openApiSchema.IsArray())
            {
                var items = openApiSchema.Items;
                items.GetSchemaMembers(path, "array", list);
            }
            // Object Type
            else if (openApiSchema.IsObject())
            {
                var props = openApiSchema.Properties;
                foreach (var prop in props)
                {
                    var propName = prop.Key;
                    var newPath = new List<string>();
                    newPath.AddRange(path);
                    newPath.Add(propName);
                    list.Add(newPath, prop.Value);
                    prop.Value.GetSchemaMembers(newPath, "object", list);
                }
            }
            // Simple Type
            else
            {
                if (parentType == "array")
                {
                    var newPath = new List<string>();
                    newPath.AddRange(path);
                    var type = openApiSchema.Type.ToLower();
                    var format = string.IsNullOrEmpty(openApiSchema.Format) ? "" : Constants.ArrayItemFormatSeparator + openApiSchema.Format.ToLower();
                    newPath.Add($"{path.Last()}{Constants.ArrayItemSeparator}{type}{format}]");
                    list.Add(newPath, openApiSchema);
                }
            }
            return list;
        }
        public static IDictionary<IList<string>, OpenApiSchema> GetMembers(this OpenApiSchema openApiSchema, bool includeMainSchema = false)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            return GetSchemaMembers(openApiSchema, includeMainSchema: includeMainSchema);
        }
        public static IList<OpenApiMemberInfo> GetMembersInfo(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            var result = new List<OpenApiMemberInfo>();
            var members = GetMembers(openApiSchema, true);
            var parentType = string.Empty;

            foreach (var member in members)
            {
                var count = member.Key.Count;
                if (count == 0)
                {
                    if (member.Value.Type == "object") parentType = "object";
                    if (member.Value.Type == "array") parentType = "array";
                    continue;
                }
                var name = member.Key.Last();
                var nameInRequired = Constants.ArrayItemRegex.IsMatch(name) ? Constants.ArrayItemRegex.Match(name).Groups[1].Value : name;
                var parents = member.Key.SkipLast(1).ToArray();

                result.Add(new OpenApiMemberInfo
                {
                    Path = member.Key.ToArray(),
                    Name = name,
                    Parents = parents,
                    Value = member.Value,
                    ParentType = parents.Length == 0
                            ? parentType
                            : result.Find(x => x.Path.Aggregate((a, b) => $"{a}.{b}") == parents.Aggregate((a, b) => $"{a}.{b}")).Type,
                    HasItems = member.Value.IsArray(),
                    HasEmptyReference = member.Value.HasEmptyReference(),
                    HasProperties = member.Value.IsObject(),
                    HasReference = member.Value.HasReference(),
                    IsPrimitive = member.Value.IsPrimitive(),
                    Format = member.Value.Format,
                    Type = member.Value.Type,
                    Required = openApiSchema.IsRequired(x => x == nameInRequired)
                });
            }
            return result;
        }
        public static void Traverse(this OpenApiSchema openApiSchema, Action<OpenApiMemberInfo> action)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            var members = GetMembersInfo(openApiSchema);
            foreach (var member in members)
            {
                action(member);
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static string ToJsonExample(this OpenApiSchema openApiSchema, Func<OpenApiMemberInfo, object> dataProvider = null)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            var objectInArray = true;
            var result = "";
            var members = openApiSchema.GetMembersInfo();
            var groupData = members.GroupBy(x => x.ParentKey).ToList();

            if (members.Count > 0)
            {
                var parentType = members.First(x => x.ParentKey == Constants.RootIndicator).ParentType;
                if (parentType == "object")
                {
                    result = $"{{ {GetReplacementKey(Constants.RootIndicator)} }}";
                }
                if (parentType == "array")
                {
                    result = $"[ {GetReplacementKey(Constants.RootIndicator)} ]";
                }
            }
            else
            {
                if (openApiSchema.IsPrimitive())
                {
                    return "\"defaultSingle\"";
                }
                return string.Empty;

            }

            foreach (var member in members)
            {
                if (member.Type == "array")
                {
                    if (!objectInArray)
                    {
                        objectInArray = true;
                    }
                    var data = $"\"{member.Name}\": [ {GetReplacementKey(member.PathKey)} ], {GetReplacementKey(member.ParentKey)}";
                    result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), data);
                }

                if (member.Type == "object")
                {
                    if (!objectInArray)
                    {
                        objectInArray = true;
                    }
                    var refData = member.HasEmptyReference ? "" : GetReplacementKey(member.PathKey);
                    var data = $"\"{member.Name}\": {{ {refData} }}, {GetReplacementKey(member.ParentKey)}";
                    result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), data);
                }

                if (member.Type != "array" && member.Type != "object")
                {
                    if (member.ParentType == "array" && member.IsArrayItem)
                    {
                        result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), "\"defaultArray\"");
                    }

                    if (member.ParentType == "array" && !member.IsArrayItem)
                    {
                        if (objectInArray)
                        {
                            result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), $"{{ {GetReplacementKey(member.ParentKey)} }}");
                            objectInArray = false;
                        }

                        var parentGroup = groupData.Find(x => x.Key == member.ParentKey);
                        var parentGroupData = parentGroup.Select(x => x.PathKey).ToList();
                        var parentGroupCount = parentGroupData.Count;
                        var isLastItem = parentGroupData.FindIndex(x => x == member.PathKey) == parentGroupCount - 1;
                        var reqKey = isLastItem ? "" : GetReplacementKey(member.ParentKey);

                        result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), $"\"{member.Name}\": \"default\", {reqKey}");
                    }
                    if (member.ParentType == "object")
                    {
                        result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), $"\"{member.Name}\": \"default\", {GetReplacementKey(member.ParentKey)}");
                    }
                }
            }
            result = Constants.JsonExampleRegex.Replace(result, string.Empty);
            result = result.ToFormattedJson();
            return result;

            string GetReplacementKey(string data) => $"{Constants.JsonExampleIndicator}{data}{Constants.JsonExampleIndicator}";
        }

    }
}

