using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Kubeleans.Impl;
using Kubeleans.Inter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
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
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(ip => ip.AdvertisedIPAddress = IPAddress.Loopback)
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
                });
            using (var silo = builder.Build())
            {
                await silo.StartAsync();
                await done.WaitAsync();
            }
        }
    }
}