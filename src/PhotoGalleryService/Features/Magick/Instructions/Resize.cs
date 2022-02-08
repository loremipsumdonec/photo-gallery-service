using PhotoGalleryService.Features.Magick.Attributes;

namespace PhotoGalleryService.Features.Magick.Instructions
{
    [Instruction("resize", "r")]
    public class Resize
        : IInstruction
    {
        private readonly int _width;
        private readonly int _height;

        public Resize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Apply(IConvertContext context)
        {
            context.Append($"-resize {_width}x{_height}");
        }
    }
}
