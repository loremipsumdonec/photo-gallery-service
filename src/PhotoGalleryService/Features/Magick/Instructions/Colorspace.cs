using PhotoGalleryService.Features.Magick.Attributes;

namespace PhotoGalleryService.Features.Magick.Instructions
{
    [Instruction("colorspace")]
    public class Colorspace
        : IInstruction
    {
        private readonly string _colorspace;

        public Colorspace(string colorspace) 
        {
            _colorspace = colorspace;
        }

        public void Apply(IConvertContext context)
        {
            context.Append($"-colorspace {_colorspace}");
        }
    }
}
