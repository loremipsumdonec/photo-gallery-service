namespace PhotoGalleryService.Features.Magick.Attributes
{
    public class InstructionAttribute
        : Attribute
    {
        public InstructionAttribute(string name)
            : this(name, name)
        {
        }

        public InstructionAttribute(string name, string shortName) 
        {
            Name = name;
            ShortName = shortName;
        }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public Type InstructionType { get; set; }
    }
}
