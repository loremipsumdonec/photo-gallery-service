using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Queries
{
    public class GetAlbum
        : Query
    {
        public GetAlbum(string albumId)
        {
            AlbumId = albumId;
        }

        public string AlbumId { get; set; }
    }

    [Handle(typeof(GetAlbum))]
    public class GetAlbumHandler
        : QueryHandler<GetAlbum>
    {
        private readonly IAlbumStorage _storage;

        public GetAlbumHandler(IAlbumStorage storage)
        {
            _storage = storage;
        }

        public override Task<IModel> ExecuteAsync(GetAlbum query)
        {
            var album = _storage.Get(query.AlbumId);

            return Task.FromResult((IModel)album);
        }
    }
}
