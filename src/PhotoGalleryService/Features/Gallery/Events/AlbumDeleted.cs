using PhotoGalleryService.Features.Gallery.Models;
using Boilerplate.Features.Reactive.Events;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class AlbumDeleted
        : Album, IEvent
    {
        public AlbumDeleted(Album source)
            : base(source)
        {
        }
    }
}
