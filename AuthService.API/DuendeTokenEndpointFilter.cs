using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class DuendeTokenEndpointFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // /connect/token endpointini ekliyoruz
        swaggerDoc.Paths.Add("/connect/token", new OpenApiPathItem
        {
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [OperationType.Post] = new OpenApiOperation
                {
                    Summary = "Duende IdentityServer Token Endpoint",
                    Description = "Client credentials veya diğer grant türleri ile token alınmasını sağlar.",
                    RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/x-www-form-urlencoded"] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Type = "object",
                                    Properties = new Dictionary<string, OpenApiSchema>
                                    {
                                        ["grant_type"] = new OpenApiSchema
                                        {
                                            Type = "string",
                                            Default = new OpenApiString("password")
                                        },
                                        ["client_id"] = new OpenApiSchema { Type = "string" ,Default = new OpenApiString("file-service-client") },
                                        ["client_secret"] = new OpenApiSchema { Type = "string",Default = new OpenApiString("supersecret")  },
                                        ["scope"] = new OpenApiSchema { Type = "string" ,Default = new OpenApiString("file-service.read") },
                                        ["username"] = new OpenApiSchema { Type = "string" },
                                        ["password"] = new OpenApiSchema { Type = "string" }  
                                    },
                                    Required = new HashSet<string> { "grant_type", "client_id", "client_secret", "scope" }
                                }
                            }
                        }
                    },
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse
                        {
                            Description = "Token response",
                            Content = new Dictionary<string, OpenApiMediaType>
                            {
                                ["application/json"] = new OpenApiMediaType
                                {
                                    Schema = new OpenApiSchema
                                    {
                                        Type = "object",
                                        // Token yanıt şemasını detaylandırabilirsiniz
                                        // Örneğin:
                                        Properties = new Dictionary<string, OpenApiSchema>
                                        {
                                            ["access_token"] = new OpenApiSchema { Type = "string" },
                                            ["expires_in"] = new OpenApiSchema { Type = "integer" },
                                            ["token_type"] = new OpenApiSchema { Type = "string" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }
}