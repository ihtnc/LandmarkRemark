using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using LandmarkRemark.Api.Http;
using LandmarkRemark.Api.Repositories;
using LandmarkRemark.Api.Security;
using LandmarkRemark.Api.Services;

namespace LandmarkRemark
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            return services
                    .AddHttpClient()

                    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()

                    .AddTransient<ISecurityService, SecurityService>()
                    .AddTransient<IRemarksService, RemarksService>()

                    .AddTransient<IFirebaseAuthenticationProvider, FirebaseAuthenticationProvider>()

                    .AddTransient<IRemarksRepository, FirebaseRemarksRepository>()

                    .AddTransient<IApiClient, ApiClient>()
                    .AddTransient<IApiRequestProvider, ApiRequestProvider>()

                    .AddTransient<IUserDetailsProvider, UserDetailsProvider>();
        }

        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, string projectId)
        {
            services
                 .AddAuthorization()
                 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.Authority = $"https://securetoken.google.com/{projectId}";
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = $"https://securetoken.google.com/{projectId}",
                         ValidateAudience = true,
                         ValidAudience = projectId,
                         ValidateLifetime = true
                     };
                 });

            return services;
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>(false);

                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "Landmark Remark API",
                    Version = "V1",
                    Description = "Backend API for the Landmark Remark app",
                    Contact = new OpenApiContact
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

        public static IApplicationBuilder UseFirebaseAuthentication(this IApplicationBuilder app)
        {
            return app.UseAuthentication()
                      .UseAuthorization();
        }
    }
}