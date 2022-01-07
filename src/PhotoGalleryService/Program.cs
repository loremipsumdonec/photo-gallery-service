using PhotoGalleryService.Features.Gallery;
using PhotoGalleryService.Features.Gallery.Events;
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
});

builder.Services.AddGraphQLServer()
    .AddQueryType<GalleryQuery>()
    .AddMutationType<GalleryMutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, configuration) =>
    {
        configuration.UseTimeout(c => c.Timeout = TimeSpan.FromSeconds(120));

        configuration.Host(builder.Configuration.GetValue<string>("message.broker-service:parameters:host"), "/", h =>
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.MapGet("/", () => "Hello PhotoGalleryService!");

app.Run();

public partial class Program { }