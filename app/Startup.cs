using AspNetCore.Firebase.Authentication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    option.Identity = Configuration["ENV_FIREBASE_IDENTITY"];
                    option.Database = Configuration["ENV_FIREBASE_DATABASE"];
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
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiDocumentation();

            services.AddFirebaseAuthentication(Configuration["Firebase:Authentication:Issuer"], Configuration["Firebase:Authentication:Audience"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials());

            app.UseApiDocumentation();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
