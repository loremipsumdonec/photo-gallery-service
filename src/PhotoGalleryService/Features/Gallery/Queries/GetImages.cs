using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Queries
{
    public class GetImages
        : Query
    {
        public GetImages(int offset, int fetch)
        {
            Offset = offset;
            Fetch = fetch;
        }

        public int Offset { get; set; }

        public int Fetch { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }

    public class GetImagesModel
        : IModel
    {
        public int Offset { get; set; }

        public int Fetch { get; set; }

        public List<Image> Images { get; set; } = new();

        public void Add(Image image)
        {
            Images.Add(image);
        }
    }

    [Handle(typeof(GetImages))]
    public class GetImagesHandler
        : QueryHandler<GetImages>
    {
        private readonly IImageStorage _storage;

        public GetImagesHandler(IImageStorage storage)
        {
            _storage = storage;
        }

        public override Task<IModel> ExecuteAsync(GetImages query)
        {
            var model = new GetImagesModel();

            foreach(var template in _storage.List(query.Offset, query.Fetch)) 
            {
                model.Add(template);
            }

            return Task.FromResult((IModel)model);
        }
    }
}
