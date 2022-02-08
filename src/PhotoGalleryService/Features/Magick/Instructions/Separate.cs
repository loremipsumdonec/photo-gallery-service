using PhotoGalleryService.Features.Magick.Attributes;

namespace PhotoGalleryService.Features.Magick.Instructions
{
    [Instruction("separate")]
    public class Separate
        : IInstruction
    {
        public void Apply(IConvertContext context)
        {
            context.Append($"-separat");
        }
    }
}
