using AspNetCore.Firebase.Authentication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LandmarkRemark.Api.Filters;

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
                .Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseApiDocumentation();

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
