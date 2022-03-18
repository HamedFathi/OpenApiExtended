using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using OpenApiExtended.Enums;
using OpenApiExtended.Utilities;

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

            var parameters = openApiOperations.SelectMany(x => x.Parameters).ToList();
            return parameters;
        }
        public static IList<OpenApiParameter> GetParameters(this IEnumerable<OpenApiOperation> openApiOperations, out int count)
        {
            if (openApiOperations == null)
            {
                throw new ArgumentNullException(nameof(openApiOperations));
            }
            var parameters = openApiOperations.SelectMany(x => x.Parameters).ToList();
            count = parameters.Count;
            return parameters;
        }
        public static IList<OpenApiParameter> GetParameters(this IEnumerable<OpenApiOperation> openApiOperations, Func<OpenApiParameter, bool> predicate)
        {
            if (openApiOperations == null)
            {
                throw new ArgumentNullException(nameof(openApiOperations));
            }

            var parameters = openApiOperations.SelectMany(x => x.Parameters).ToList();

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
            var parameters = openApiOperations.SelectMany(x => x.Parameters).ToList();
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
        public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, Func<CommonMimeType, bool> predicate)
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
                .Where(x => predicate((CommonMimeType)ConvertToCommonMimeType<CommonMimeType>(x.Key)))
                .Select(x => x.Value)
                .ToList()
                ;
            return apiMediaTypes;
        }
        public static IList<OpenApiMediaType> GetRequestBodyContent(this OpenApiOperation openApiOperation, Func<CommonMimeType, bool> predicate, out int count)
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
                .Where(x => predicate((CommonMimeType)ConvertToCommonMimeType<CommonMimeType>(x.Key)))
                .Select(x => x.Value).ToList();
            count = apiMediaTypes.Count;
            return apiMediaTypes;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static object ConvertToCommonMimeType<T>(string mimeType) where T : Enum
        {
            var result = Helper.GetEnumInfo<T>().FirstOrDefault(x => x.Description == mimeType);
            if (result == null)
            {
                return null;
            }
            return (T)Enum.Parse(typeof(T), result.Value, true);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, Func<CommonMimeType, bool> predicate)
        {
            if (openApiOperation == null)
            {
                throw new ArgumentNullException(nameof(openApiOperation));
            }

            var requestBody = openApiOperation.RequestBody;

            var apiMediaTypes = requestBody?.Content
                    .Where(x => predicate((CommonMimeType)ConvertToCommonMimeType<CommonMimeType>(x.Key)))
                    .Select(x => x.Value)
                    .ToList()
                ;

            var schemas = apiMediaTypes?.Select(x => x.Schema).ToList();
            return schemas;
        }
        public static IList<OpenApiSchema> GetRequestBodySchema(this OpenApiOperation openApiOperation, Func<CommonMimeType, bool> predicate, out int count)
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
                .Where(x => predicate((CommonMimeType)ConvertToCommonMimeType<CommonMimeType>(x.Key)))
                .Select(x => x.Value).ToList();
            count = apiMediaTypes.Count;
            var schemas = apiMediaTypes.Select(x => x.Schema).ToList();
            return schemas;
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------




    }
}
