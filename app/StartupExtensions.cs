using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace LandmarkRemark
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>(false);

                c.SwaggerDoc("V1", new Info
                {
                    Title = "Landmark Remark API",
                    Version = "V1",
                    Description = "Backend API for the Landmark Remark app",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Art Amurao"
                    }
                });
            });
        }

        public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app)
        {
            return app.UseSwagger()
                      .UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"Landmark Remark V1"); });
        }
    }
}