using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moonlay.Confluent.Kafka;
using Moonlay.Core.Models;
using Moonlay.WebApp.Clients;

namespace Moonlay.WebApp
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
            services.AddHttpContextAccessor();

            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IManageDataSetClient>(c => new ManageDataSetClient(Grpc.Net.Client.GrpcChannel.ForAddress(Configuration.GetSection("Grpc:ServerUrl").Value)));

            services.AddRazorPages();

            ConfigureKafka(services);

            services.AddMetrics();
        }

        private void ConfigureKafka(IServiceCollection services)
        {
            services.AddSingleton(c => new SchemaRegistryConfig
            {
                Url = Configuration.GetSection("Kafka:SchemaRegistryUrl").Value,
                // Note: you can specify more than one schema registry url using the
                // schema.registry.url property for redundancy (comma separated list). 
                // The property name is not plural to follow the convention set by
                // the Java implementation.
                // optional schema registry client properties:
                RequestTimeoutMs = 5000,
                MaxCachedSchemas = 10
            });

            services.AddSingleton<ISchemaRegistryClient>(c => new CachedSchemaRegistryClient(c.GetRequiredService<SchemaRegistryConfig>()));
            services.AddSingleton(c => new ProducerConfig() { BootstrapServers = Configuration.GetSection("Kafka:BootstrapServers").Value });

            services.AddScoped<IKafkaProducer, KafkaProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
