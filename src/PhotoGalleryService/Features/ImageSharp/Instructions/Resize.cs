using PhotoGalleryService.Features.ImageSharp.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PhotoGalleryService.Features.ImageSharp.Instructions
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

        protected override void ApplySynchronously(Image image)
        {
            var options = new ResizeOptions()
            {
                Size = new Size(_width, _height)
            };

            image.Mutate(i => i.Resize(options));
        }
    }
}
