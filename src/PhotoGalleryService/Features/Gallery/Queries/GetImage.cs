using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Queries
{
    public class GetImage
        : Query
    {
        public GetImage(string imageId)
        {
            ImageId = imageId;
        }

        public string ImageId { get; set; }
    }

    [Handle(typeof(GetImage))]
    public class GetImageHandler
        : QueryHandler<GetImage>
    {
        private readonly IImageStorage _storage;

        public GetImageHandler(IImageStorage storage)
        {
            _storage = storage;
        }

        public override Task<IModel> ExecuteAsync(GetImage query)
        {
            var image = _storage.Get(query.ImageId);

            return Task.FromResult((IModel)image);
        }
    }
}
