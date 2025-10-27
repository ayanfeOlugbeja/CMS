using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CMS.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddEnhancedSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CMS Employee Management API",
                    Version = "v1",
                    Description = "A comprehensive Employee Management System API with role-based access control",
                    Contact = new OpenApiContact
                    {
                        Name = "CMS Development Team",
                        Email = "support@cms.com"
                    }
                });

                // Add JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}' to authenticate"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                // Include XML comments if available
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Add response examples
                c.SchemaFilter<ResponseExampleSchemaFilter>();
            });
        }
    }

    public class ResponseExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DTOs.ApiResponse))
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["success"] = new Microsoft.OpenApi.Any.OpenApiBoolean(true),
                    ["message"] = new Microsoft.OpenApi.Any.OpenApiString("Operation successful"),
                    ["data"] = new Microsoft.OpenApi.Any.OpenApiObject(),
                    ["timestamp"] = new Microsoft.OpenApi.Any.OpenApiString(DateTime.UtcNow.ToString("O"))
                };
            }
        }
    }
}
