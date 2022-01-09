using PhotoGalleryService.Features.Gallery.Services;
using Autofac;
using Boilerplate.Features.Core.Config;

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

            builder.RegisterFromAs<IAlbumStorage>(
                "gallery.album.storage",
                Configuration
            ).InstancePerLifetimeScope();

            builder.RegisterFromAs<IImageStorage>(
                "gallery.image.storage",
                Configuration
            ).InstancePerLifetimeScope();

            builder.RegisterFromAs<IImageFileStorage>(
                "gallery.image.file.storage",
                Configuration
            ).InstancePerLifetimeScope();
        }

        private void ValidateConfiguration() 
        {
            IEnumerable<string> keys = new List<string>()
            {
                "gallery.album.storage:parameters:hostname",
                "gallery.album.storage:parameters:username",
                "gallery.album.storage:parameters:password",
                "gallery.album.storage:parameters:credentialDatabaseName",
                "gallery.album.storage:parameters:databaseName",
                "gallery.image.storage:parameters:hostname",
                "gallery.image.storage:parameters:username",
                "gallery.image.storage:parameters:password",
                "gallery.image.storage:parameters:credentialDatabaseName",
                "gallery.image.storage:parameters:databaseName",
                "gallery.image.file.storage:parameters:hostname",
                "gallery.image.file.storage:parameters:username",
                "gallery.image.file.storage:parameters:password",
                "gallery.image.file.storage:parameters:credentialDatabaseName",
                "gallery.image.file.storage:parameters:databaseName"
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
