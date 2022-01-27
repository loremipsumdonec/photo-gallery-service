using MongoDB.Driver.GridFS;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public interface IImageFileStorage
    {
        void Clear();

        GridFSFileInfo Get(string imageId);

        string Upload(string imageId, byte[] data, Action<GridFSUploadOptions> build = null);

        byte[] Download(string imageId);

        void Delete(string imageId);
    }
}
