using PhotoGalleryService.Features.Gallery.Models;
using Boilerplate.Features.Reactive.Events;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class ImageDeleted
        : Image, IEvent
    {
        public ImageDeleted(Image source)
            : base(source)
        {
        }
    }
}
