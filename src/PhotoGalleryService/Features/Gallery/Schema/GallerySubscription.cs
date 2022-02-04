using PhotoGalleryService.Features.Gallery.Events;

namespace PhotoGalleryService.Features.Gallery.Schema
{
    public class GallerySubscription
    {
        [Subscribe]
        public ImageCreated OnImageCreated([EventMessage] ImageCreated @event) => @event;

        [Subscribe]
        public ImageDeleted OnImageDeleted([EventMessage] ImageDeleted @event) => @event;

        [Subscribe]
        public ImageFileUploaded OnImageFileUploaded([EventMessage] ImageFileUploaded @event) => @event;

        [Subscribe]
        public ImageFileDeleted OnImageFileDeleted([EventMessage] ImageFileDeleted @event) => @event;

        [Subscribe]
        public ImageUpdated OnImageUpdated([EventMessage] ImageUpdated @event) => @event;
    }
}
