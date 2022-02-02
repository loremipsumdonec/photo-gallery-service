using Boilerplate.Features.RabbitMQ.Services;
using Boilerplate.Features.Testing.Services;

namespace PhotoGalleryServiceTest.Services;

public class PhotoGalleryServiceEngineForSmoke
    : PhotoGalleryServiceEngine
{
    public PhotoGalleryServiceEngineForSmoke()
        : base()
    {
    }

    protected override DistributedServiceEngine CreateDistributedServiceEngine()
    {
        var configuration = GetConfiguration();
        string @namespace = configuration.GetSection("kubernetes:namespace").Value;

        var composite = new CompositeReadinessProbe();
        composite.Add(new RabbitMQReadinessProbe(configuration.GetSection("message.broker-service:parameters")));

        return new KubernetesDistributedServiceEngine(
            System.IO.Directory.GetCurrentDirectory(), 
            @namespace, 
            composite
        );
    }
}

