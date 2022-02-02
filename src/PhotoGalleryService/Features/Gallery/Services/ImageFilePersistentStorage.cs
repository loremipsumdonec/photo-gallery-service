
namespace PhotoGalleryService.Features.Gallery.Services
{
    public class ImageFilePersistentStorage
        : IImageFileStorage
    {
        private readonly string _storageMountPath;

        public ImageFilePersistentStorage(string storageMountPath) 
        {
            _storageMountPath = storageMountPath;
        }

        public void Clear() 
        {
        }

        public void Upload(string imageId, byte[] data) 
        {
            string path = System.IO.Path.Combine(_storageMountPath, imageId);
            File.WriteAllBytes(path, data);
        }

        public byte[] Download(string imageId) 
        {
            string path = System.IO.Path.Combine(_storageMountPath, imageId);

            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }

            return null;
        }

        public void Delete(string imageId) 
        {
            string path = System.IO.Path.Combine(_storageMountPath, imageId);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
