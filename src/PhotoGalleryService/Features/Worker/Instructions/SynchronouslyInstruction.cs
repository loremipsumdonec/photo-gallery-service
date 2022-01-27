using ImageMagick;

namespace PhotoGalleryService.Features.Worker.Instructions
{
    public abstract class SynchronouslyInstruction
        : IInstruction
    {
        public async Task ApplyAsync(MagickImage image)
        {
            await Task.Run(() => ApplySynchronously(image));
        }

        protected abstract void ApplySynchronously(MagickImage image);
    }
}
