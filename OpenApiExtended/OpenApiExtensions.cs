using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

// ReSharper disable UnusedMember.Global

namespace OpenApiExtended
{
    public static class OpenApiExtensions
    {
        public static OpenApiDocument ToOpenApiDocument(this string openApiDocumentText, out OpenApiDiagnostic diagnostic)
        {
            if (string.IsNullOrWhiteSpace(openApiDocumentText))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(openApiDocumentText));

            var openApiDocument = new OpenApiStreamReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(openApiDocumentText)), out diagnostic);
            return openApiDocument;
        }
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
        public static IList<OpenApiSecurityScheme> GetOpenApiSecuritySchemes(this OpenApiDocument openApiDocument)
        {
            var result = new List<OpenApiSecurityScheme>();
            foreach (var securityRequirement in openApiDocument.SecurityRequirements)
            {
                foreach (var req in securityRequirement)
                {
                    if (req.Key != null)
                    {
                        result.Add(req.Key);
                    }
                }
            }
            return result;
        }
        public static IList<OpenApiSecurityScheme> GetOpenApiSecuritySchemes(this OpenApiDocument openApiDocument, Func<OpenApiSecurityScheme, bool> predicate)
        {
            var result = new List<OpenApiSecurityScheme>();
            foreach (var securityRequirement in openApiDocument.SecurityRequirements)
            {
                foreach (var req in securityRequirement)
                {
                    if (req.Key != null)
                    {
                        var status = predicate(req.Key);
                        if (status)
                            result.Add(req.Key);
                    }
                }
            }
            return result;
        }
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
        public static bool HasParameter(this OpenApiOperation openApiOperation)
        {
            if (openApiOperation == null)
            {
                throw new ArgumentNullException(nameof(openApiOperation));
            }
            return openApiOperation.Parameters != null && openApiOperation.Parameters.Any();
        }
        public static bool IsCookie(this OpenApiParameter openApiParameter)
        {
            if (openApiParameter == null)
            {
                throw new ArgumentNullException(nameof(openApiParameter));
            }

            return openApiParameter.In == ParameterLocation.Cookie;
        }
        public static bool IsHeader(this OpenApiParameter openApiParameter)
        {
            if (openApiParameter == null)
            {
                throw new ArgumentNullException(nameof(openApiParameter));
            }

            return openApiParameter.In == ParameterLocation.Header;
        }
        public static bool IsPath(this OpenApiParameter openApiParameter)
        {
            if (openApiParameter == null)
            {
                throw new ArgumentNullException(nameof(openApiParameter));
            }

            return openApiParameter.In == ParameterLocation.Path;
        }
        public static bool IsQuery(this OpenApiParameter openApiParameter)
        {
            if (openApiParameter == null)
            {
                throw new ArgumentNullException(nameof(openApiParameter));
            }

            return openApiParameter.In == ParameterLocation.Query;
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
        private static object ConvertToOpenApiMimeType<T>(string mimeType) where T : Enum
        {
            var result = Extensions.GetEnumInfo<T>().FirstOrDefault(x => x.Description == mimeType);
            if (result == null)
            {
                return null;
            }
            return result.Item;
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
        public static bool IsEmptyObject(this OpenApiSchema openApiSchema)
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
        private static IDictionary<IList<string>, OpenApiSchema> GetSchemaMembers(this OpenApiSchema openApiSchema, IList<string> path = null, string parentType = null, IDictionary<IList<string>, OpenApiSchema> list = null)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            path ??= new List<string>();
            list ??= new Dictionary<IList<string>, OpenApiSchema>();

            // Array Type
            if (openApiSchema.IsArray())
            {
                if (list.Count == 0)
                {
                    var objPath = new List<string>();
                    objPath.AddRange(path);
                    list.Add(objPath, openApiSchema);
                }

                var items = openApiSchema.Items;
                items.GetSchemaMembers(path, "array", list);
            }
            // Object Type
            else if (openApiSchema.IsObject())
            {
                var objPath = new List<string>();
                objPath.AddRange(path);
                list.Add(objPath, openApiSchema);

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
                    newPath.Add($"{Constants.ArrayItemIndicator}{type}{format}]");
                    list.Add(newPath, openApiSchema);
                }
            }
            return list;
        }
        public static IDictionary<IList<string>, OpenApiSchema> GetMembers(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }

            var result = GetSchemaMembers(openApiSchema)
                .DistinctBy(x => x.Value)
                .ToDictionary(x => x.Key, y => y.Value);
            return result;
        }
        public static void TraverseOnJsonPaths(this OpenApiSchema openApiSchema, Action<string, OpenApiSchema> action)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            var members = GetMembers(openApiSchema);
            foreach (var member in members)
            {
                var key = member.Key.Count == 0 ? Constants.RootIndicator : Constants.RootIndicator + "." + member.Key.Aggregate((a, b) =>
                      $"{a}.{b}");
                key = key.Contains(Constants.ArrayItemIndicator) ? key.Replace(Constants.ArrayItemIndicator, "[") : key;
                action(key, member.Value);
            }
        }
        public static void TraverseOnMembers(this OpenApiSchema openApiSchema, Action<OpenApiMemberInfo> action)
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
        public static IList<OpenApiMemberInfo> GetMembersInfo(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            var result = new List<OpenApiMemberInfo>();
            var members = GetMembers(openApiSchema);
            foreach (var member in members)
            {
                var name = member.Key.Count == 0 ? null : member.Key.Last();
                var parents = member.Key.Count == 0 ? null : member.Key.Count == 1 ? Array.Empty<string>() : member.Key.SkipLast(1).ToArray();
                // ReSharper disable once UseObjectOrCollectionInitializer
                var item = new OpenApiMemberInfo();
                item.Path = member.Key.Count == 0 ? null : member.Key.ToArray();
                item.Name = name;
                item.Parents = parents;
                item.Value = member.Value;
                item.ParentType = parents == null ? null : parents.Length == 0
                    ? members.Values.First().Type
                    : name != null && name.StartsWith(Constants.ArrayItemIndicator)
                        ? "array"
                        : result.First(x => x.PathKey == x.ParentKey).Type;
                item.IsArray = member.Value.IsArray();
                item.IsEmptyObject = member.Value.IsEmptyObject();
                item.IsObject = member.Value.IsObject();
                item.HasReference = member.Value.HasReference();
                item.ReferenceId = member.Value.HasReference() ? member.Value.Reference.Id : null;
                item.IsPrimitive = member.Value.IsPrimitive();
                item.Format = member.Value.Format;
                item.Type = member.Value.Type;
                item.Required = openApiSchema.Required.ToArray();
                item.IsOptional = openApiSchema.Required.All(x => x != name);
                item.IsNullable = member.Value.Nullable;
                item.IsRoot = member.Key.Count == 0;
                result.Add(item);
            }
            return result;
        }
        public static string ToJson(this OpenApiSchema openApiSchema, Func<OpenApiMemberInfo, object> dataProvider = null)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }
            Regex regex = new("___.+?___", RegexOptions.Compiled);
            var result = "";
            var members = openApiSchema.GetMembersInfo();

            var groupedMembers = members.GroupBy(x => x.ParentKey).ToList();

            if (members.Count == 0)
            {
                if (openApiSchema.IsPrimitive())
                {
                    var value = dataProvider?.Invoke(null).ToString() ?? openApiSchema.GetOpenApiSchemaDefaultValue().ToString();
                    return value;
                }
                return string.Empty;
            }

            var counter = 0;
            foreach (var member in members)
            {
                if (member.IsRoot)
                {
                    if (member.Type == "object")
                    {
                        result = $"{{ {GetReplacementKey(Constants.RootIndicator)} }}";
                    }
                    if (member.Type == "array")
                    {
                        result = $"[ {GetReplacementKey(Constants.RootIndicator)} ]";
                    }
                }
                else
                {
                    if (member.Type == "array")
                    {
                        var data = $"\"{member.Name}\": [ {GetReplacementKey(member.PathKey)} ], {GetReplacementKey(member.ParentKey)}";
                        result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), data);
                    }

                    if (member.Type == "object")
                    {
                        var prevItemWithSameName = members[counter - 1].PathKey == member.PathKey && members[counter - 1].Type == "array";
                        if (prevItemWithSameName)
                        {
                            result = result.ReplaceFirst(GetReplacementKey(member.PathKey), $"{{ {GetReplacementKey(member.PathKey)} }}");
                        }
                        else
                        {
                            var refData = member.IsEmptyObject ? "" : GetReplacementKey(member.PathKey);
                            var data = $"\"{member.Name}\": {{ {refData} }}, {GetReplacementKey(member.ParentKey)}";
                            result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), data);
                        }
                    }
                    if (member.Type != "array" && member.Type != "object")
                    {
                        if (member.ParentType == "array" && member.IsArrayItem)
                        {
                            var value = dataProvider?.Invoke(member).ToString() ?? member.Value.GetOpenApiSchemaDefaultValue().ToString();
                            result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), value);
                        }

                        if (member.ParentType == "array" && !member.IsArrayItem)
                        {
                            var parentGroup = groupedMembers.First(x => x.Key == member.ParentKey);
                            var parentGroupData = parentGroup.Select(x => x.PathKey).ToList();
                            var parentGroupCount = parentGroupData.Count;
                            var isLastItem = parentGroupData.FindIndex(x => x == member.PathKey) == parentGroupCount - 1;
                            var reqKey = isLastItem ? "" : GetReplacementKey(member.ParentKey);

                            var value = dataProvider?.Invoke(member).ToString() ?? member.Value.GetOpenApiSchemaDefaultValue().ToString();
                            result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), $"\"{member.Name}\": {value}, {reqKey}");
                        }
                        if (member.ParentType == "object")
                        {
                            var value = dataProvider?.Invoke(member).ToString() ?? member.Value.GetOpenApiSchemaDefaultValue().ToString();
                            result = result.ReplaceFirst(GetReplacementKey(member.ParentKey), $"\"{member.Name}\": {value}, {GetReplacementKey(member.ParentKey)}");
                        }
                    }
                }
                counter++;
            }
            result = regex.Replace(result, string.Empty);
            result = result.ToFormattedJson();
            return result;

            string GetReplacementKey(string data) => $"___{data}___";
        }
        public static JsonNode ToJsonNode(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }

            return JsonNode.Parse(openApiSchema.ToJson());
        }
        private static string GetTypeScriptData(this OpenApiSchema openApiSchema, out IList<TypeScriptInfo> typeScriptInfo, string rootName = "Root", TypeScriptConfiguration typeScriptConfiguration = null)
        {
            if (openApiSchema == null)
            {
                throw new ArgumentNullException(nameof(openApiSchema));
            }

            typeScriptConfiguration ??= new TypeScriptConfiguration();
            var export = typeScriptConfiguration.ExportWithDefault ? "export default" : "export";
            var prefixData = typeScriptConfiguration.StartInterfaceNameWithI ? "I" : string.Empty;
            typeScriptInfo = new List<TypeScriptInfo>();
            var members = openApiSchema.GetMembersInfo();
            var source = "";
            Regex regex = new("_>_.+?_<_", RegexOptions.Compiled);
            if (members.Count == 0)
            {

                return string.Empty;
            }
            foreach (var member in members)
            {
                if (member.IsRoot)
                {
                    var sb = new StringBuilder();
                    var name = GetPascalName(rootName, prefixData);
                    sb.AppendLine($"{export} interface {name} {{");
                    sb.AppendLine($"{GetReplacementKey(Constants.RootIndicator)}", 1);
                    sb.AppendLine("}");
                    typeScriptInfo.Add(new TypeScriptInfo { Name = name, PathKey = Constants.RootIndicator });
                    source = sb.ToString();
                }
                else
                {
                    if (member.IsPrimitive && !member.IsArrayItem)
                    {
                        source = source.ReplaceFirst(GetReplacementKey(member.ParentKey), GetTypeScriptMember(member) + $"\r\n\t{GetReplacementKey(member.ParentKey)}");
                        var model = typeScriptInfo.SingleOrDefault(x => x.PathKey == member.ParentKey);
                        if (model != null)
                        {
                            model.Members.Add(new TypeScriptMemberInfo
                            {
                                Name = member.Name,
                                IsOptional = member.IsOptional,
                                IsNullable = member.IsNullable,
                                Type = new TypeScriptMemberInfoType
                                {
                                    Format = member.Format,
                                    Value = member.Type,
                                    DataType = member.Type
                                }
                            });
                        }
                    }

                    if (member.IsPrimitive && member.IsArrayItem)
                    {
                        var name = members.First(x => x.PathKey == member.ParentKey && x.IsArray).Name;
                        source = source.ReplaceFirst(GetReplacementKey(member.ParentKey), GetTypeScriptMember(member, member.Type + "[]", name) + $"\r\n\t{GetReplacementKey(member.ParentKey)}");
                    }

                    if (member.IsObject)
                    {
                        var isEmptyObject = member.IsEmptyObject;
                        var hasArrayParent = members.Any(x => x.PathKey == member.PathKey && x.IsArray);
                        if (!hasArrayParent)
                        {
                            var type = isEmptyObject && typeScriptConfiguration.ReplaceEmptyObjectsWithUnknownType
                                ? "unknown"
                                : GetPascalName(member.Name);

                            var model = typeScriptInfo.SingleOrDefault(x => x.PathKey == member.ParentKey);
                            if (model != null)
                            {
                                model.Members.Add(new TypeScriptMemberInfo
                                {
                                    Name = member.Name,
                                    IsOptional = member.IsOptional,
                                    IsNullable = member.IsNullable,
                                    Type = new TypeScriptMemberInfoType
                                    {
                                        Format = member.Format,
                                        Value = member.Type,
                                        DataType = type
                                    }
                                });
                            }

                            source = source.ReplaceFirst(GetReplacementKey(member.ParentKey), GetTypeScriptMember(member, type) + $"\r\n\t{GetReplacementKey(member.ParentKey)}");
                        }
                        if (!(isEmptyObject && typeScriptConfiguration.ReplaceEmptyObjectsWithUnknownType))
                        {
                            var sb = new StringBuilder();
                            var name = GetPascalName(member.Name, prefixData);
                            sb.AppendLine($"{export} interface {name} {{");
                            sb.AppendLine($"{GetReplacementKey(member.PathKey)}", 1);
                            sb.AppendLine("}");
                            typeScriptInfo.Add(new TypeScriptInfo { Name = name, PathKey = member.PathKey });
                            source += sb.ToString();
                        }
                    }

                    if (member.IsArray)
                    {
                        var objectInArray = members.FirstOrDefault(x => x.PathKey == member.PathKey && x.IsObject);
                        var isSimpleArray = objectInArray == null;
                        if (isSimpleArray)
                        {
                            source = source.ReplaceFirst(GetReplacementKey(member.ParentKey), GetReplacementKey(member.PathKey) + $"\r\n\t{GetReplacementKey(member.ParentKey)}");
                        }
                        else
                        {
                            var name = GetPascalName(objectInArray.Name, prefixData);
                            var type = name + "[]";
                            var model = typeScriptInfo.SingleOrDefault(x => x.PathKey == objectInArray.ParentKey);
                            if (model != null)
                            {
                                model.Members.Add(new TypeScriptMemberInfo
                                {
                                    Name = member.Name,
                                    IsOptional = objectInArray.IsOptional,
                                    IsNullable = objectInArray.IsNullable,
                                    Type = new TypeScriptMemberInfoType
                                    {
                                        Format = member.Format,
                                        Value = member.Type,
                                        DataType = type
                                    }
                                });
                            }
                            source = source.ReplaceFirst(GetReplacementKey(member.ParentKey), GetTypeScriptMember(member, type) + $"\r\n\t{GetReplacementKey(member.ParentKey)}");
                        }
                    }
                }
            }

            source = regex.Replace(source, string.Empty);
            source = source.Split('\n').Where(x => !string.IsNullOrEmpty(x.Trim()))
                .Aggregate((a, b) => a + '\n' + b).Trim();
            return source;

            string GetReplacementKey(string data) => $"_>_{data}_<_";
            string GetOptionalSign(OpenApiMemberInfo openApiMemberInfo) => openApiMemberInfo.IsOptional ? "?" : string.Empty;
            string GetPascalName(string name, string prefix = null, bool singular = true)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) return name;

                prefix = string.IsNullOrEmpty(prefix) ? string.Empty : prefix;

                var result = name.RemoveDuplicateWhiteSpaces().Trim();
                var info = CultureInfo.InvariantCulture.TextInfo;
                result = info.ToTitleCase(result).Replace(" ", string.Empty);
                result = singular ? result.Singularize() : result;
                result = prefix + result;
                result = result.EndsWith("id")
                    ? result.Substring(0, result.Length - 2) + "Id"
                    : result;
                return result;
            }
            string GetTypeScriptMember(OpenApiMemberInfo member, string type = null, string simpleArrayName = null)
            {
                var tsType = OpenApiUtility.GetTypeScriptType(OpenApiUtility.GetOpenApiValueType(member.Type, member.Format));
                var interfaceType = string.IsNullOrEmpty(type) ? tsType : type;
                if (member.IsArrayItem && string.IsNullOrEmpty(simpleArrayName))
                {
                    throw new ArgumentNullException(nameof(simpleArrayName));
                }
                var name = !member.IsArrayItem ? member.Name : simpleArrayName;
                var nullable = member.IsNullable ? " | undefined" : string.Empty;
                var result = $"{name}{GetOptionalSign(member)}: {interfaceType}{nullable};";
                return result;
            }
        }
        public static TypeScriptResult ToTypeScript(this OpenApiSchema openApiSchema, string rootName = "Root",
            TypeScriptConfiguration typeScriptConfiguration = null)
        {
            if (openApiSchema == null) throw new ArgumentNullException(nameof(openApiSchema));

            var ts = openApiSchema.GetTypeScriptData(out var tsModel, rootName, typeScriptConfiguration);
            if (string.IsNullOrEmpty(ts))
            {
                return null;
            }
            var indexFile = new StringBuilder();
            var tsData = new List<TypeScriptData>();
            var imports = new StringBuilder();
            var hasMultiResult = IsInterfaceLine(ts, out _);
            var interfaceName = string.Empty;
            if (hasMultiResult)
            {
                var localList = new List<string>();
                var lines = ts.Split('\n');
                foreach (var line in lines)
                {
                    var status = IsInterfaceLine(line, out var name);
                    var isDefault = IsExportDefault(line);
                    if (status)
                    {
                        localList.Clear();
                    }

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        interfaceName = name;
                        var model = isDefault ? "*" : $"{{ {name} }}";
                        indexFile.AppendLine($"export {model} from './{name}';");
                    }

                    var memberRegex = new Regex(@"\s*(.+)\:");
                    var member = memberRegex.Match(line);
                    localList.Add(line);
                    if (member.Success)
                    {
                        var currentModel =
                            tsModel.Where(x => x.Name == interfaceName).SelectMany(x => x.Members)
                                .FirstOrDefault(x => x.Name == member.Groups[1].Value.TrimEnd('?'));
                        if (currentModel == null) continue;
                        if (!string.IsNullOrWhiteSpace(currentModel.Type.Format)) continue;

                        if (currentModel.Type.Value == "object" || currentModel.Type.Value == "array")
                        {
                            var type = currentModel.Type.DataType.Replace("[", "").Replace("]", "");
                            var model = isDefault ? "*" : $"{{ {type} }}";
                            imports.AppendLine($"import {model} from './{type}';");
                        }
                    }
                    if (line.Trim() == "}")
                    {
                        var data = imports.Length > 0 ? imports.ToString().Trim() + "\n" : string.Empty;

                        tsData.Add(new TypeScriptData
                        {
                            TypeScriptInfo = tsModel.Single(x => x.Name == interfaceName),
                            Source = data + localList.Aggregate((a, b) => a + '\n' + b).Trim()
                        });
                        imports.Clear();
                    }
                }
            }
            else
            {
                tsData.Add(new TypeScriptData
                {
                    Source = ts,
                    TypeScriptInfo = null
                });
            }

            var result = new TypeScriptResult
            {
                Index = indexFile.Length > 0 ? indexFile.ToString() : null,
                TypeScriptData = tsData
            };
            return result;

            bool IsInterfaceLine(string code, out string name)
            {
                name = string.Empty;
                var status = code.Contains("export interface") || code.Contains("export default interface");
                if (status)
                {
                    var regex = new Regex(@"\s*export\s*(default)?\s*interface\s*(.+)\s*\{\s*");
                    var match = regex.Match(code);
                    name = match.Groups[2].Value.Trim();
                }
                return status;
            }
            bool IsExportDefault(string code)
            {
                return code.Contains("export default interface");
            }
        }
        private static object GetOpenApiSchemaDefaultValue(this OpenApiSchema openApiSchema)
        {
            if (openApiSchema.IsPrimitive())
            {
                var info = OpenApiUtility.GetOpenApiValueType(openApiSchema.Type, openApiSchema.Format);
                switch (info)
                {
                    case OpenApiValueType.Boolean:
                        return "true";
                    case OpenApiValueType.Integer:
                        return 0;
                    case OpenApiValueType.Int32:
                        return 0;
                    case OpenApiValueType.Int64:
                        return 0;
                    case OpenApiValueType.Number:
                        return 1.0;
                    case OpenApiValueType.Float:
                        return 1.0;
                    case OpenApiValueType.Double:
                        return 1.0;
                    case OpenApiValueType.String:
                        return "\"string\"";
                    case OpenApiValueType.Date:
                        return $"\"{DateTime.Now:yyyy -MM-dd}\"";
                    case OpenApiValueType.DateTime:
                        return $"\"{DateTime.Now:yyyy-MM-ddTHH:mm:ssZ}\"";
                    case OpenApiValueType.Password:
                        return "\"P@ssW0rd\"";
                    case OpenApiValueType.Byte:
                        return "\"ZGVmYXVsdA==\"";
                    case OpenApiValueType.Email:
                        return "\"test@example.com\"";
                    case OpenApiValueType.Uuid:
                        return $"\"{Guid.NewGuid()}\"";
                    case OpenApiValueType.Uri:
                        return "\"https://www.example.com/\"";
                    case OpenApiValueType.HostName:
                        return "\"https://www.domain.example.com/\"";
                    case OpenApiValueType.IPv4:
                        return "\"127.0.0.1\"";
                    case OpenApiValueType.IPv6:
                        return "\"::1\"";
                    case OpenApiValueType.Null:
                        return null;
                    default:
                        return string.Empty;
                }
            }
            return null;

        }
        public static void Traverse(this OpenApiDocument openApiDocument, Action<string, OpenApiMemberType, object> action)
        {
            if (openApiDocument == null)
            {
                throw new ArgumentNullException(nameof(openApiDocument));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            var paths = openApiDocument.GetPaths();
            foreach (var path in paths)
            {
                var pathKey = path.Key;
                var pathValue = path.Value;
                action(pathKey, OpenApiMemberType.Path, pathValue);
                foreach (var op in pathValue.Operations)
                {
                    var operationKey = op.Key.ToString().ToLower();
                    var reqBodyKey = pathKey + ">" + operationKey;
                    var parameters = op.Value.Parameters;
                    if (parameters is { Count: > 0 })
                    {
                        foreach (var item in parameters)
                        {
                            action(reqBodyKey, OpenApiMemberType.Parameter, item);
                        }
                    }
                    var reqBody = op.Value.RequestBody;
                    if (reqBody != null)
                    {
                        var data = reqBody.GetRequestBodySchema(x => x == OpenApiMimeType.ApplicationJson);
                        action(reqBodyKey, OpenApiMemberType.RequestBody, reqBody);
                        foreach (var item in data)
                        {
                            var members = item.GetMembersInfo();
                            action(reqBodyKey, OpenApiMemberType.RequestBodyMembers, members);
                        }
                    }
                    var responses = op.Value.Responses;
                    foreach (var response in responses)
                    {
                        var responseKey = reqBodyKey + ">" + response.Key;
                        var res = response.Value;
                        action(responseKey, OpenApiMemberType.Response, res);
                        if (res != null)
                        {
                            var data = res.GetResponsesSchema(x => x == OpenApiMimeType.ApplicationJson);
                            foreach (var item in data)
                            {
                                var members = item.GetMembersInfo();
                                action(responseKey, OpenApiMemberType.ResponseMembers, members);
                            }
                        }
                    }
                }
            }
        }
        public static bool IsListOfOpenApiMemberInfo(this object obj)
        {
            return obj is IList<OpenApiMemberInfo>;
        }
        public static bool IsOpenApiMemberInfo(this object obj)
        {
            return obj is OpenApiMemberInfo;
        }
        public static bool IsOpenApiSchema(this object obj)
        {
            return obj is OpenApiSchema;
        }
        public static bool IsOpenApiPathItem(this object obj)
        {
            return obj is OpenApiPathItem;
        }
        public static bool IsOpenApiRequestBody(this object obj)
        {
            return obj is OpenApiRequestBody;
        }
        public static bool IsOpenApiResponses(this object obj)
        {
            return obj is OpenApiResponses;
        }
        public static bool IsOpenApiResponse(this object obj)
        {
            return obj is OpenApiResponse;
        }
    }
}