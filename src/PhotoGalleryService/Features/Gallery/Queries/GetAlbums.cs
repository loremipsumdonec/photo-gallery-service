using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Queries
{
    public class GetAlbums
        : Query
    {
        public GetAlbums(int offset, int fetch)
        {
            Offset = offset;
            Fetch = fetch;
        }

        public int Offset { get; set; }

        public int Fetch { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }

    public class GetAlbumsModel
        : IModel
    {
        public int Offset { get; set; }

        public int Fetch { get; set; }

        public List<Album> Albums { get; set; } = new();

        public void Add(Album template)
        {
            Albums.Add(template);
        }
    }

    [Handle(typeof(GetAlbums))]
    public class GetAlbumsHandler
        : QueryHandler<GetAlbums>
    {
        private readonly IAlbumStorage _storage;

        public GetAlbumsHandler(IAlbumStorage storage)
        {
            _storage = storage;
        }

        public override Task<IModel> ExecuteAsync(GetAlbums query)
        {
            var model = new GetAlbumsModel();

            foreach(var template in _storage.List(query.Offset, query.Fetch)) 
            {
                model.Add(template);
            }

            return Task.FromResult((IModel)model);
        }
    }
}
