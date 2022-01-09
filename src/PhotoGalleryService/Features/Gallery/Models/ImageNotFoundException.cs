namespace PhotoGalleryService.Features.Gallery.Models
{
    public sealed class ImageNotFoundException
        : Exception
    {
        public ImageNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
