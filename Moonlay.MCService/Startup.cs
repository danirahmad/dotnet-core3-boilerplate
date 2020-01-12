using Confluent.Kafka;
using Confluent.SchemaRegistry;
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Moonlay.Core.Models;
using Moonlay.MCServiceWebApi.Consumers;
using Moonlay.MCServiceWebApi.Customers.GraphQL;
using Moonlay.MCServiceWebApi.Db;
using Moonlay.MCServiceGRPC;

namespace Moonlay.MCServiceWebApi
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

            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddDbContext<MyDbTrailContext>(options => options.UseSqlServer(Configuration.GetSection("ConnectionStrings:ConnectionTrail").Value));

            services.AddScoped<IDbContext, MyDbContext>();
            services.AddScoped<IDbTrailContext, MyDbTrailContext>();

            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<Customers.ICustomerRepository, Customers.Repository>();
            services.AddScoped<Customers.ICustomerService, Customers.Service>();


            ConfigureRestFullServices(services);

            //ConfigureGraphQL(services);

            ConfigureKafka(services);

            services.AddMetrics();

            services.AddHttpContextAccessor();
            services.AddGrpc();
        }

        private void ConfigureKafka(IServiceCollection services)
        {
            services.AddHostedService<HostedConsumers>();

            services.AddSingleton(c => new SchemaRegistryConfig
            {
                Url = "192.168.99.100:8081",
                // Note: you can specify more than one schema registry url using the
                // schema.registry.url property for redundancy (comma separated list). 
                // The property name is not plural to follow the convention set by
                // the Java implementation.
                // optional schema registry client properties:
                RequestTimeoutMs = 5000,
                MaxCachedSchemas = 10
            });
            services.AddSingleton<ISchemaRegistryClient>(c => new CachedSchemaRegistryClient(c.GetRequiredService<SchemaRegistryConfig>()));

            services.AddSingleton(c => new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "192.168.99.100:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            });
            services.AddScoped<INewCustomerConsumer, NewCustomerConsumer>();
            services.AddScoped<IUpdateCustomerConsumer, UpdateCustomerConsumer>();
        }

        private void ConfigureRestFullServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Moonlay Web API BoilerPlate",
                    Contact = new OpenApiContact()
                    {
                        Name = "Afandy Lamusu",
                        Email = "afandy.lamusu@moonlay.com",
                        // Url = new Uri("www.dotnetdetail.net", UriKind.RelativeOrAbsolute)
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Moonlay",
                        // Url = new Uri("www.dotnetdetail.net", UriKind.RelativeOrAbsolute)
                    },
                });
            });
        }

        private void ConfigureGraphQL(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            // services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            // services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<CustomerType>();
            // services.AddSingleton<StarWarsMutation>();
            // services.AddSingleton<HumanType>();
            // services.AddSingleton<HumanInputType>();
            // services.AddSingleton<DroidType>();
            // services.AddSingleton<CharacterInterface>();
            // services.AddSingleton<EpisodeEnum>();

            services.AddSingleton<DQuery>();
            services.AddSingleton<DMutation>();
            services.AddSingleton<ISchema, DSchema>();

            services.AddGraphQL(_ =>
            {
                _.EnableMetrics = true;
                _.ExposeExceptions = true;
            })
            .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // app.UseCookiePolicy();

            app.UseRouting();
            // app.UseRequestLocalization();
            // app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseSession();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Map("/grpc", c =>
            {
                c.UseRouting();
                c.UseEndpoints(ep =>
                {
                    // ENABLE GRPC
                    ep.MapGrpcService<GreeterService>();
                });
            });



            // add http for Schema at default url /graphql
            // app.UseGraphQL<ISchema>();

            // if (env.IsDevelopment())
            // use graphql-playground at default url /ui/playground
            // app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
        }
    }
}
