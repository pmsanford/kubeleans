using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kubeleans.Inter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

namespace Kubeleans.WebApp
{
    public class Startup
    {
        private static IClusterClient BuildOrleansClient()
        {
            var endpointIPStr = Environment.GetEnvironmentVariable("ORL_GATEWAY_IP");
            var endpointPortStr = Environment.GetEnvironmentVariable("ORL_GATEWAY_PORT");
            var endpointIP = IPAddress.Parse(endpointIPStr);
            var endpointPort = int.Parse(endpointPortStr);
            var endpoint = new IPEndPoint(endpointIP, endpointPort);
            var builder = new ClientBuilder()
                .Configure<ClusterOptions>(opts =>
                {
                    opts.ClusterId = "dev";
                })
                .UseStaticClustering(endpoint)
                .ConfigureApplicationParts(config =>
                {
                    config.AddApplicationPart(typeof(IContentGrain).Assembly);
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                });
            var client = builder.Build();
            client.Connect().Wait();
            return client;
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var client = BuildOrleansClient();
            services.AddMvc();
            services.AddSingleton<IClusterClient>(client);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
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
