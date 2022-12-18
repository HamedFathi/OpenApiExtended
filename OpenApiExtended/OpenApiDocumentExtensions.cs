// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using OpenApiExtended.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
    public static object? FindByKey(this OpenApiDocument openApiDocument, string key, string separator = "<>")
    {
        return openApiDocument.FindByKey(key, out _, separator);
    }

    public static object? FindByKey(this OpenApiDocument openApiDocument, string key, out OpenApiKeyType openApiKeyType, string separator = "<>")
    {
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (string.IsNullOrEmpty(separator))
        {
            throw new ArgumentNullException(nameof(separator));
        }
        var parts = key.Split(new[] { separator }, StringSplitOptions.None);
        var count = parts.Length;
        OpenApiPathItem? path = null;
        OpenApiOperation? operation = null;
        OpenApiResponse? response = null;
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
            operation = path.Operations.FirstOrDefault(x => string.Equals(x.Key.ToString(), parts[1], StringComparison.OrdinalIgnoreCase)).Value;
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

        return count switch
        {
            1 => path,
            2 => operation,
            3 => response,
            _ => null
        };
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

    public static string GetDescription(this OpenApiDocument openApiDocument)
    {
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }
        return openApiDocument.Info.Description;
    }

    public static OpenApiInfo GetInfo(this OpenApiDocument openApiDocument)
    {
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }
        return openApiDocument.Info;
    }

    public static OpenApiLicense GetLicense(this OpenApiDocument openApiDocument)
    {
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }
        return openApiDocument.Info.License;
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

    public static IList<string> GetOperationsKeys(this OpenApiDocument openApiDocument, string separator = "->")
    {
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }
        separator = string.IsNullOrEmpty(separator) ? "->" : separator;
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

    public static IList<string> GetResponsesKeys(this OpenApiDocument openApiDocument, string separator = "->")
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

    public static Version? GetSemanticVersion(this OpenApiDocument openApiDocument)
    {
        if (openApiDocument == null)
        {
            throw new ArgumentNullException(nameof(openApiDocument));
        }

        var versionInfo = openApiDocument.Info.Version;

        try
        {
            return Version.Parse(versionInfo);
        }
        catch
        {
            return null;
        }
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

    public static OpenApiDocument ToOpenApiDocument(this string openApiDocumentText,
                                    out OpenApiDiagnostic diagnostic)
    {
        if (string.IsNullOrWhiteSpace(openApiDocumentText))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(openApiDocumentText));

        return new OpenApiStreamReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(openApiDocumentText)), out diagnostic);
    }

    public static void Traverse(this string openApiText, Action<string, object> action)
    {
        var openApiDocument = ToOpenApiDocument(openApiText, out _);

        foreach (var path in openApiDocument.Paths)
        {
            var val = path.Value;
            action(path.Key, val);

            foreach (var ext in val.Extensions)
            {
                action(ext.Key, ext.Value);
            }

            foreach (var parameter in val.Parameters)
            {
                if (parameter != null)
                {
                    action(parameter.Name, parameter);
                }
            }

            foreach (var operation in val.Operations)
            {
                action(operation.Key.ToString(), operation.Value);
            }

            foreach (var server in val.Servers)
            {
                action(server.Url, server);
            }

            action(val.Reference.Id, val.Reference);
        }
    }
}