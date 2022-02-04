using PhotoGalleryService.Features.Worker.Attributes;
using SixLabors.ImageSharp;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("format","f")]
    public class Convert
        : SynchronouslyInstruction
    {

        public Convert(string format) 
        {
        }

        protected override void ApplySynchronously(Image image)
        {
        }
    }
}
