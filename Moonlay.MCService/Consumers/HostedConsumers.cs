
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moonlay.MasterData.WebApi.Consumers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi
{
    public class HostedConsumers : IHostedService, IDisposable
    {
        private readonly ILogger<HostedConsumers> _logger;
        public HostedConsumers(IServiceProvider services,
            ILogger<HostedConsumers> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Kafka Consumers Hosted Service is running.");

            Task.Run(async () => await DoConsumers(stoppingToken), stoppingToken);

            return Task.CompletedTask;
        }

        private Task DoConsumers(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var tasks = new Task[] {
                    Task.Run(async()=>await scope.ServiceProvider.GetRequiredService<INewCustomerConsumer>().Run(stoppingToken)),
                    Task.Run(async()=>await scope.ServiceProvider.GetRequiredService<IUpdateCustomerConsumer>().Run(stoppingToken))
                };

                Task.WaitAll(tasks);
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Kafka Consumers Hosted Service is stopping.");

            await Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }

}