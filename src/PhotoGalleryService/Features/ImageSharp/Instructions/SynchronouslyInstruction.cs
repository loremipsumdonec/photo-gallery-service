using SixLabors.ImageSharp;

namespace PhotoGalleryService.Features.ImageSharp.Instructions
{
    public abstract class SynchronouslyInstruction
        : IInstruction
    {
        public async Task ApplyAsync(Image image)
        {
            await Task.Run(() => ApplySynchronously(image));
        }

        protected abstract void ApplySynchronously(Image image);
    }
}
