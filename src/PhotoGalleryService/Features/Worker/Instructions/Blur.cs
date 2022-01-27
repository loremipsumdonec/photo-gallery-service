using ImageMagick;
using PhotoGalleryService.Features.Worker.Attributes;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("blur", "b")]
    public class Blur
        : SynchronouslyInstruction
    {
        private readonly double _radius;
        private readonly double _sigma;

        public Blur(double radius, double sigma) 
        { 
            _radius = radius;
            _sigma = sigma;
        }

        protected override void ApplySynchronously(MagickImage image)
        {
            image.Blur(_radius, _sigma);
        }
    }
}
