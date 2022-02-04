using PhotoGalleryService.Features.Worker.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("grayscale")]
    public class Grayscale
        : SynchronouslyInstruction
    {
        protected override void ApplySynchronously(Image image)
        {
            image.Mutate(i => i.Grayscale());
        }
    }
}
