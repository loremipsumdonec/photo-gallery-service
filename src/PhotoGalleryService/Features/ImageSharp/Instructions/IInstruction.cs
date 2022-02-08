using SixLabors.ImageSharp;

namespace PhotoGalleryService.Features.ImageSharp.Instructions
{
    public interface IInstruction
    {
        Task ApplyAsync(Image image);
    }
}
