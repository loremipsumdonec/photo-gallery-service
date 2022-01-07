using PhotoGalleryService.Features.Gallery.Models;
using Boilerplate.Features.Reactive.Events;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class ImageUpdated
        : Image, IEvent
    {
        public ImageUpdated(Image source)
            : base(source)
        {
        }
    }
}
