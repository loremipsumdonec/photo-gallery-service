using Boilerplate.Features.Reactive.Events;
using PhotoGalleryService.Features.Gallery.Models;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class ImageFileUploaded
        : Image, IEvent
    {
        public ImageFileUploaded(Image source)
            : base(source)
        {
        }
    }
}
