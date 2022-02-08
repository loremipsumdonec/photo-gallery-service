using PhotoGalleryService.Features.ImageSharp.Attributes;
using SixLabors.ImageSharp;

namespace PhotoGalleryService.Features.ImageSharp.Instructions
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
