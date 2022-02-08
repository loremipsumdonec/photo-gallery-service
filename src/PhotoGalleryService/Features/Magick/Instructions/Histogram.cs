
using PhotoGalleryService.Features.Magick.Attributes;

namespace PhotoGalleryService.Features.Magick.Instructions
{
    [Instruction("histogram")]
    public class Histogram
        : IOutputInstruction 
    {
        public bool UniqueColors { get; set; }

        public void Apply(IConvertContext context)
        {
            context.Format = "gif";
            context.Append($"-define histogram:unique-colors={UniqueColors} histogram:{context.Out}");
        }
    }
}
