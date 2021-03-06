using Elastic.Apm.Api;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public class ImageFileStorageWithTracer
        : IImageFileStorage
    {
        private readonly IImageFileStorage _decorated;
        private readonly ITracer _tracer;

        public ImageFileStorageWithTracer(IImageFileStorage decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public void Clear()
        {
            _tracer.CurrentTransaction.CaptureSpan("IImageFileStorage/Clear", "storage", _decorated.Clear);
        }

        public void Delete(string imageId)
        {
            _tracer.CurrentTransaction.CaptureSpan("IImageFileStorage/Delete", "storage", () => _decorated.Delete(imageId));
        }

        public byte[] Download(string imageId)
        {
            return _tracer.CurrentTransaction.CaptureSpan("IImageFileStorage/Download", "storage", () => _decorated.Download(imageId));
        }

        public void Upload(string imageId, byte[] data)
        {
            _tracer.CurrentTransaction.CaptureSpan("IImageFileStorage/Upload", "storage", () => _decorated.Upload(imageId, data));
        }
    }
}
