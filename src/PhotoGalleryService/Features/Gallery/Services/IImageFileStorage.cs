namespace PhotoGalleryService.Features.Gallery.Services
{
    public interface IImageFileStorage
    {
        void Clear();

        string Upload(string imageId, byte[] data);

        byte[] Download(string imageId);

        void Delete(string imageId);
    }
}
