using ImageMagick;
using PhotoGalleryService.Features.Worker.Attributes;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("resize", "r")]
    public class Resize
        : SynchronouslyInstruction
    {
        private readonly int _width;
        private readonly int _height;

        public Resize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        protected override void ApplySynchronously(MagickImage image)
        {
            image.Resize(_width, _height);
        }
    }
}
