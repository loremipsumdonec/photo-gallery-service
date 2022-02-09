using PhotoGalleryService.Features.Gallery.Services;
using Autofac;
using Boilerplate.Features.Core.Config;
using StackExchange.Redis;

namespace PhotoGalleryService.Features.Gallery
{
    public class GalleryModule
        : Autofac.Module
    {
        public GalleryModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected override void Load(ContainerBuilder builder)
        {
            ValidateConfiguration();

            RegisterGalleryImageFileStorage(builder);

            builder.RegisterFromAs<IImageStorage>(
                "gallery.image.storage",
                Configuration
            ).InstancePerLifetimeScope();

            builder.RegisterFromAs<IImageFileStorage>(
                "gallery.image.file.storage",
                Configuration
            ).InstancePerLifetimeScope();
        }

        private void RegisterGalleryImageFileStorage(ContainerBuilder builder) 
        {
            string hostname = Configuration.GetValue<string>("gallery.image.file.storage:parameters:hostname");
            int port = Configuration.GetValue<int>("gallery.image.file.storage:parameters:port");

            var l = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                AllowAdmin = true,
                EndPoints = { $"{hostname}:{port}" },
                ConnectTimeout = 30000,
                SyncTimeout = 30000
            });

            builder.RegisterInstance<IConnectionMultiplexer>(l)
                .Named<IConnectionMultiplexer>("gallery.image.file.storage")
                .SingleInstance();
        }

        private void ValidateConfiguration() 
        {
            IEnumerable<string> keys = new List<string>()
            {
                "gallery.image.storage:parameters:hostname",
                "gallery.image.storage:parameters:username",
                "gallery.image.storage:parameters:password",
                "gallery.image.storage:parameters:credentialDatabaseName",
                "gallery.image.storage:parameters:databaseName"
            };

            foreach(string key in keys) 
            {
                if(string.IsNullOrEmpty(Configuration.GetValue<string>(key)))
                {
                    throw new ArgumentNullException(key, $"Missing configuration {key}");
                }
            }
        }
    }
}
