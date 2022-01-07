using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Queries;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Schema
{
    public class GalleryQuery
    {
        public Task<Album> Album(string albumId, [Service] IQueryDispatcher dispatcher) 
        {
            return dispatcher.DispatchAsync<Album>(
                new GetAlbum(albumId)
            );
        }

        public Task<GetAlbumsModel> Albums(int offset, int fetch, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetAlbumsModel>(new GetAlbums(offset, fetch));
        }
    }
}
