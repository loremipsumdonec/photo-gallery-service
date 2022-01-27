using ImageMagick;
using PhotoGalleryService.Features.Worker.Attributes;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("grayscale")]
    public class Grayscale
        : SynchronouslyInstruction
    {
        protected override void ApplySynchronously(MagickImage image)
        {
            image.Grayscale();
        }
    }
}
