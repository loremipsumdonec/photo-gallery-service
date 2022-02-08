using PhotoGalleryService.Features.Magick.Attributes;

namespace PhotoGalleryService.Features.Magick.Instructions
{
    [Instruction("sepia-tone")]
    public class SepiaTone
        : IInstruction
    {
        private readonly double _percent;

        public SepiaTone(double percent)
        {
            _percent = percent;
        }

        public void Apply(IConvertContext context)
        {
            context.Append($"-sepia-tone {_percent}%");
        }
    }
}
