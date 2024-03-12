using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Notes.WebApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiProvider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiProvider)
    {
        _apiProvider = apiProvider;
    }
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiProvider.ApiVersionDescriptions) 
        {
            var apiVersion = description.ApiVersion.ToString();
            options.SwaggerDoc(description.GroupName,
                new OpenApiInfo
                {
                    Version = apiVersion,
                    Title = $"Notes API {apiVersion}",
                    Description =
                        "A example asp .net core for greatest future",
                    TermsOfService = null, 
                    Contact = new OpenApiContact
                    {
                        Name = "My VK",
                        Url =
                            new Uri("https://vk.com/pufpufenjoyer")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Telegram @pufpufenjoyer",
                        Url = new Uri("https://t.me/pufpufenjoyer")
                    }
                });
            options.AddSecurityDefinition($"AuthToken {apiVersion}", 
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    Description = "Authorization token"

                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = $"AuthToken {apiVersion}"
                        }
                    },
                new string[]  { }
                }
            });

            options.CustomOperationIds(apiDescription =>
            apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
            ? methodInfo.Name
            : null);
        }
    }
}
