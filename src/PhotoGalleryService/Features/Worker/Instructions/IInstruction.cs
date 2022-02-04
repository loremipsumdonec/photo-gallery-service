using SixLabors.ImageSharp;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    public interface IInstruction
    {
        Task ApplyAsync(Image image);
    }
}
