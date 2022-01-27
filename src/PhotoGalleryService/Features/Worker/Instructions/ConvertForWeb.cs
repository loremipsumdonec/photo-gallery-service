using ImageMagick;
using PhotoGalleryService.Features.Worker.Attributes;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("web")]
    public class ConvertForWeb
        : SynchronouslyInstruction
    {
        protected override void ApplySynchronously(MagickImage image)
        {
            image.Strip();
        }
    }
}
