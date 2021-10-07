using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LandmarkRemark.Api.Config;
using LandmarkRemark.Api.Filters;
using LandmarkRemark.Api.Middlewares;

namespace LandmarkRemark.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .Configure<FirebaseConfig>(option =>
                {
                    option.ProjectId = Configuration["ENV_FIREBASE_PROJECT_ID"];
                    option.AuthEndpoint = Configuration["ENV_FIREBASE_AUTH_ENDPOINT"];
                    option.DbEndPoint = Configuration["ENV_FIREBASE_DB_ENDPOINT"];
                    option.ApiKey = Configuration["ENV_FIREBASE_APIKEY"];
                })
                .Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddDependencies();

            services
                .AddCors()
                .AddMvc(config =>
                {
                    config.Filters.Add(new AuthoriseFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    config.Filters.Add<ModelValidationFilter>();
                });

            services.AddApiDocumentation();

            services.AddFirebaseAuthentication(Configuration["ENV_FIREBASE_PROJECT_ID"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .SetIsOriginAllowed(hostname => true));

            app.UseRouting();

            app.UseApiDocumentation();

            app.UseFirebaseAuthentication();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
