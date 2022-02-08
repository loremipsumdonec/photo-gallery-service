using PhotoGalleryService.Features.ImageSharp.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PhotoGalleryService.Features.ImageSharp.Instructions
{
    [Instruction("blur", "b")]
    public class Blur
        : SynchronouslyInstruction
    {
        private readonly double _sigma;

        public Blur(double sigma) 
        { 
            _sigma = sigma;
        }

        protected override void ApplySynchronously(Image image)
        {
            image.Mutate(i => i.GaussianBlur((float)_sigma));
        }
    }
}
