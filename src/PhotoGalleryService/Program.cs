using PhotoGalleryService.Features.Gallery;
using PhotoGalleryService.Features.Gallery.Schema;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper;
using Boilerplate.Features.MassTransit.Services;
using Boilerplate.Features.Reactive.Reactive;
using MassTransit;
using System.Reflection;
using Boilerplate.Features.MassTransit;
using RemotePhotographer.Features.Photographer.Events;
using PhotoGalleryService.Features.Worker;
using MassTransit.MongoDbIntegration.MessageData;
using PhotoGalleryService.Features.Gallery.Queries;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer((ContainerBuilder containerBuilder) =>
{
    List<Assembly> assemblies = new List<Assembly>();
    assemblies.Add(Assembly.Load(new AssemblyName("PhotoGalleryService")));
    assemblies.Add(Assembly.Load(new AssemblyName("Boilerplate")));

    containerBuilder.RegisterModule(new CoreModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new MapperModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new ReactiveModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new MassTransitModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new GalleryModule(builder.Configuration));
    containerBuilder.RegisterModule(new WorkerModule(builder.Configuration, assemblies));
});

builder.Services.AddControllers();
builder.Services.AddInMemorySubscriptions();

builder.Services.AddGraphQLServer()
    .AddQueryType<GalleryQuery>()
    .AddSubscriptionType<GallerySubscription>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EventConsumer<ImageCaptured>>();
    x.AddConsumer<EventConsumer<PreviewImageCaptured>>();

    x.AddConsumer<QueryConsumer<GetImages>>();

    x.UsingRabbitMq((context, configuration) =>
    {
        configuration.UseJsonSerializer();

        configuration.UseMessageData(
            new MongoDbMessageDataRepository(
                builder.Configuration.GetValue<string>("message.broker-service:parameters:data.repository.connectionString"),
                builder.Configuration.GetValue<string>("message.broker-service:parameters:data.repository.database")
            )
        );

        configuration.UseTimeout(c => c.Timeout = TimeSpan.FromSeconds(120));
        configuration.Host(
            builder.Configuration.GetValue<string>("message.broker-service:parameters:host"),
            builder.Configuration.GetValue<ushort>("message.broker-service:parameters:port"),
             "/", h =>
             {
                 h.Username(builder.Configuration.GetValue<string>("message.broker-service:parameters:username"));
                 h.Password(builder.Configuration.GetValue<string>("message.broker-service:parameters:password"));
             });

        configuration.ReceiveEndpoint(builder.Configuration.GetValue<string>("message.broker-service:parameters:receive.endpoint"), e =>
        {
            e.ConfigureConsumers(context);
        });
    });
}).AddMassTransitHostedService();

builder.Services.AddGenericRequestClient();

var app = builder.Build();
app.UseRouting();
app.UseWebSockets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapControllers();
});

app.MapGet("/", () => "photo gallery service");

app.Run();

public partial class Program { }