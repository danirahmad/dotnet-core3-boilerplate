
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moonlay.MCServiceConsumers.Consumers;

namespace Moonlay.MCService
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
                var newCustomerConsumer = scope.ServiceProvider.GetRequiredService<INewCustomerConsumer>();

                var tasks = new Task[] {
                    Task.Run(async ()=> await newCustomerConsumer.Run(scope, stoppingToken), stoppingToken),
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