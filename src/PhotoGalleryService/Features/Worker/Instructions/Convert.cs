using ImageMagick;
using PhotoGalleryService.Features.Worker.Attributes;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    [Instruction("format","f")]
    public class Convert
        : SynchronouslyInstruction
    {
        private readonly MagickFormat _format;

        public Convert(string format) 
        {
            _format = Enum.Parse<MagickFormat>(format, true);
        }

        protected override void ApplySynchronously(MagickImage image)
        {
            image.Format = _format;
        }
    }
}
