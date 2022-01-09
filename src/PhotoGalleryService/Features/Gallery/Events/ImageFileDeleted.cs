using Boilerplate.Features.Reactive.Events;
using PhotoGalleryService.Features.Gallery.Models;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class ImageFileDeleted
        : Image, IEvent
    {
        public ImageFileDeleted(Image source)
            : base(source)
        {
        }
    }
}
