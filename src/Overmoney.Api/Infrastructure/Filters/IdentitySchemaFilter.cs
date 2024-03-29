using Microsoft.OpenApi.Models;
using Overmoney.Domain.Features.Common.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Overmoney.Api.Infrastructure.Filters;

public class IdentitySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.BaseType is not null && context.Type.BaseType == typeof(Identity<int>))
        {
            schema.Type = "integer";
            schema.Format = "int32";
            schema.Minimum = 1;
            schema.Description = $"Unique identifier of type integer";
        }

        if (context.Type.BaseType is not null && context.Type.BaseType == typeof(Identity<long>))
        {
            schema.Type = "integer";
            schema.Format = "int64";
            schema.Minimum = 1;
            schema.Description = $"Unique identifier of type long";
        }
    }
}
