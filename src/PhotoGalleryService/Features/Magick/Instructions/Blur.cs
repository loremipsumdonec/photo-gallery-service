
using PhotoGalleryService.Features.Magick.Attributes;

namespace PhotoGalleryService.Features.Magick.Instructions
{
    [Instruction("blur", "b")]
    public class Blur
        : IInstruction
    {
        private readonly int _radius;
        private readonly int _sigma;

        public Blur(int radius, int sigma) 
        {
            _radius = radius;
            _sigma = sigma;
        }

        public void Apply(IConvertContext context)
        {
            context.Append($"-blur {_radius}x{_sigma}");
        }
    }
}
