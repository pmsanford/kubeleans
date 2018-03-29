using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
                .ConfigureApplicationParts(config => { /* TODO */ })
                .ConfigureLogging(logger =>
                {
                    logger.AddConsole();
                });
            using (var silo = builder.Build())
            {
                await silo.StartAsync();
                await done.WaitAsync();
            }
        }
    }
}