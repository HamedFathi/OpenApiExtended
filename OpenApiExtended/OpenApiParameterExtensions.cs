// ReSharper disable UnusedMember.Global
using Microsoft.OpenApi.Models;
using System;

namespace OpenApiExtended;

public static partial class OpenApiExtensions
{
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
}