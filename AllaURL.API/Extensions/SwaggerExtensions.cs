
using AllaURL.Common;
using AllaURL.Domain.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace AllaURL.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddEnumStringRepresentation(this SwaggerGenOptions options)
        {
            // Ensuring enums are displayed as strings in Swagger UI
            options.MapType<TokenType>(() => new OpenApiSchema
            {
                Type = "string",  // Represent the enum as a string
                Enum = Enum.GetValues(typeof(TokenType))
                    .Cast<TokenType>()
                    .Select(e => new OpenApiString(e.ToString())) // Convert to OpenApiString
                    .Cast<IOpenApiAny>()  // Cast each to IOpenApiAny, which is required by Swagger
                    .ToList()
            });
        }
    }
}
