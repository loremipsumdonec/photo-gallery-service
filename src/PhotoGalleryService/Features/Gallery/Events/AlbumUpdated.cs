using PhotoGalleryService.Features.Gallery.Models;
using Boilerplate.Features.Reactive.Events;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class AlbumUpdated
        : Album, IEvent
    {
        public AlbumUpdated(Album source)
            : base(source)
        {
        }
    }
}
