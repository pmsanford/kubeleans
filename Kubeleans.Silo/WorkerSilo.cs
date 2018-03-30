using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Kubeleans.Impl;
using Kubeleans.Inter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Kubeleans.Silo
{
    public class WorkerSilo
    {
        private readonly SemaphoreSlim done;

        public WorkerSilo()
        {
            this.done = new SemaphoreSlim(0);
        }

        public Task StopSilo()
        {
            done.Release();
            return Task.CompletedTask;
        }

        public async Task StartSilo()
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(opts => opts.ClusterId = "dev")
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000, listenOnAnyHostAddress: true)
                .UseKubeMembership(opts =>
                {
                    opts.CanCreateResources = true;
                })
                .AddMemoryGrainStorageAsDefault()
                .ConfigureApplicationParts(config =>
                {
                    config.AddApplicationPart(typeof(ContentGrain).Assembly).WithReferences();
                })
                .ConfigureLogging(logger =>
                {
                    logger.AddConsole();
                })
                .ConfigureServices(svc =>
                {
                    svc.AddSingleton<IConfig, Config>();
                    svc.AddSingleton<ISiloInfo>(new SiloInfo { SiloID = new Random().Next(0, 1000).ToString() });
                });
            using (var silo = builder.Build())
            {
                await silo.StartAsync();
                await done.WaitAsync();
            }
        }
    }
}