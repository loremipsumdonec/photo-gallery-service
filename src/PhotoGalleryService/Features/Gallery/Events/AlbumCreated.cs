using PhotoGalleryService.Features.Gallery.Models;
using Boilerplate.Features.Reactive.Events;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class AlbumCreated
        : Album, IEvent
    {
        public AlbumCreated(Album source)
            : base(source)
        {
        }
    }
}
