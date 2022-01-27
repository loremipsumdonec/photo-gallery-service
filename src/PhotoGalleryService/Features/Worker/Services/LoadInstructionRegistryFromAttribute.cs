using Boilerplate.Features.Core;
using PhotoGalleryService.Features.Worker.Attributes;
using PhotoGalleryService.Features.Worker.Instructions;
using System.Reflection;

namespace PhotoGalleryService.Features.Worker.Services
{
    public class LoadInstructionRegistryFromAttribute        
        : IInstructionRegistry
    {
        private readonly IInstructionRegistry _decorated;
        private readonly IAssemblies _assemblies;
        private readonly object _door = new object();
        private bool _isLoaded;

        public LoadInstructionRegistryFromAttribute(IInstructionRegistry decorated, IAssemblies assemblies)
        {
            _decorated = decorated;
            _assemblies = assemblies;
        }

        public void Add(string name, string shortName, Type type)
        {
            _decorated.Add(name, shortName, type);
        }

        public Type GetInstruction(string name)
        {
            LoadIfNotLoaded();
            return _decorated.GetInstruction(name);
        }

        private void LoadIfNotLoaded()
        {
            if (!_isLoaded)
            {
                lock (_door)
                {
                    if (!_isLoaded)
                    {
                        ForceLoad();
                        _isLoaded = true;
                    }
                }
            }
        }

        private void ForceLoad()
        {
            foreach (Assembly assembly in _assemblies.Get())
            {
                try
                {
                    var types = assembly.GetExportedTypes()
                                        .Where(t => typeof(IInstruction).IsAssignableFrom(t) && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes<InstructionAttribute>();

                        foreach (var attribute in attributes)
                        {
                            Add(attribute.Name, attribute.ShortName, type);
                        }
                    }
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}
