using PhotoGalleryService.Features.ImageSharp.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PhotoGalleryService.Features.ImageSharp.Instructions
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
