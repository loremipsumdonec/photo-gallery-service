using PhotoGalleryService.Features.ImageSharp.Attributes;

namespace PhotoGalleryService.Features.Magick.Services
{
    public class DefaultInstructionRegistry
        : IInstructionRegistry
    {
        private readonly List<InstructionAttribute> _registry;

        public DefaultInstructionRegistry()
        {
            _registry = new List<InstructionAttribute>();
        }

        public void Add(string name, string shortName, Type type)
        {
            _registry.Add(new InstructionAttribute(name, shortName)
            {
                InstructionType = type
            });
        }

        public Type GetInstruction(string name)
        {
            var exists = _registry.Find(i => i.Name.Equals(name) || i.ShortName.Equals(name));

            if (exists != null)
            {
                return exists.InstructionType;
            }

            return null;
        }
    }
}
