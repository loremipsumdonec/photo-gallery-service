using Elastic.Apm.Api;
using PhotoGalleryService.Features.Gallery.Models;
using System.Linq.Expressions;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public class ImageStorageWithTracer
        : IImageStorage
    {
        private readonly IImageStorage _decorated;
        private readonly ITracer _tracer;

        public ImageStorageWithTracer(IImageStorage decorated, ITracer tracer)
        {
            _decorated = decorated;
            _tracer = tracer;
        }

        public void Clear()
        {
            _tracer.CurrentTransaction.CaptureSpan("IImageStorage/Clear", "storage", _decorated.Clear);
        }

        public Image Create(Action<Image> action)
        {
            return _tracer.CurrentTransaction.CaptureSpan("IImageStorage/Create", "storage", () => _decorated.Create(action));
        }

        public Image Delete(string imageId)
        {
            return _tracer.CurrentTransaction.CaptureSpan("IImageStorage/Delete", "storage", () => _decorated.Delete(imageId));
        }

        public Image Get(string imageId)
        {
            return _tracer.CurrentTransaction.CaptureSpan("IImageStorage/Get", "storage", () => _decorated.Get(imageId));
        }

        public List<Image> List(int offset, int fetch, Expression<Func<Image, bool>> filter = null)
        {
            return _tracer.CurrentTransaction.CaptureSpan("IImageStorage/List", "storage", () => _decorated.List(offset, fetch, filter);
        }

        public Image Update(string imageId, Action<Image> action)
        {
            return _tracer.CurrentTransaction.CaptureSpan("IImageStorage/Update", "storage", () => _decorated.Update(imageId, action));
        }
    }
}
