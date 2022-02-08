using Autofac;
using Autofac.Features.AttributeFilters;
using Boilerplate.Features.Core.Config;
using PhotoGalleryService.Features.Magick.Instructions;
using PhotoGalleryService.Features.Magick.Services;
using System.Reflection;

namespace PhotoGalleryService.Features.Magick
{
    public class MagickModule
            : Autofac.Module
    {
        public MagickModule(
            IConfiguration configuration, 
            List<Assembly> assemblies)
        {
            Configuration = configuration;
            Assemblies = assemblies;
        }

        public IConfiguration Configuration { get; }

        public List<Assembly> Assemblies { get; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterFromAs<IInstructionRegistry>(
                "magick.instruction.registry",
                Configuration
            ).SingleInstance();

            builder.RegisterFromAs<IInstructionFactory>(
                "magick.instruction.factory",
                Configuration
            ).InstancePerLifetimeScope();

            foreach (Type instructionTypes in GetTypes<IInstruction>())
            {
                builder.RegisterType(instructionTypes).WithAttributeFiltering();
            }
        }

        private IEnumerable<Type> GetTypes<T>()
        {
            foreach (Assembly assembly in Assemblies)
            {
                var types = assembly.GetExportedTypes()
                    .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }
    }
}
