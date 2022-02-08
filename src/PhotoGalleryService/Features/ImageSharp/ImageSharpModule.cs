using Autofac;
using Autofac.Features.AttributeFilters;
using Boilerplate.Features.Core.Config;
using PhotoGalleryService.Features.ImageSharp.Instructions;
using PhotoGalleryService.Features.ImageSharp.Services;
using System.Reflection;

namespace PhotoGalleryService.Features.ImageSharp
{
    public class ImageSharpModule
            : Autofac.Module
    {
        public ImageSharpModule(
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
                "imagesharp.instruction.registry",
                Configuration
            ).SingleInstance();

            builder.RegisterFromAs<IInstructionFactory>(
                "imagesharp.instruction.factory",
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
