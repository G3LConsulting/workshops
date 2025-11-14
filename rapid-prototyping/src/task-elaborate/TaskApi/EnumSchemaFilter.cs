using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Schema filter to add enum value descriptions to OpenAPI documentation
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            foreach (var enumValue in Enum.GetValues(context.Type))
            {
                schema.Enum.Add(new OpenApiString(enumValue.ToString()));
            }
        }
    }
}
