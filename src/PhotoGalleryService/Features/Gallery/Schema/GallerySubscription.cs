using PhotoGalleryService.Features.Gallery.Events;

namespace PhotoGalleryService.Features.Gallery.Schema
{
    public class GallerySubscription
    {
        [Subscribe]
        public ImageCreated OnImageCreated([EventMessage] ImageCreated @event) => @event;
    }
}
