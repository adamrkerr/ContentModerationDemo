using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentModerationDemo.Abstraction;
using ContentModerationDemo.AWS;
using ContentModerationDemo.Azure;
using ContentModerationDemo.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentModerationDemo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.keys.json", optional: true) //Not included in repo, create your own
                .AddEnvironmentVariables();

            var config = builder.Build();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IAzureContentModerator>((provider) => {
                return new AzureContentModerator(Configuration.GetSection("Azure:Region").Value, Configuration.GetSection("Azure:Key").Value); }
            );
            services.AddScoped<IAWSContentModerator>((provider) => {
                return new AWSContentModerator(Configuration.GetSection("AWS:Region").Value);
            });
            services.AddScoped<IGoogleContentModerator>((provider) => {
                return new GoogleContentModerator("google-service-account.json");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
