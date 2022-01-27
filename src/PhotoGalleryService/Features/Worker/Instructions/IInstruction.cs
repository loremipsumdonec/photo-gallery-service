using ImageMagick;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    public interface IInstruction
    {
        Task ApplyAsync(MagickImage image);
    }
}
