using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Queries
{
    public class GetImages
        : Query
    {
        public GetImages()
        {
        }

        public GetImages(int offset, int fetch, IEnumerable<string> tags)
        {
            Offset = offset;
            Fetch = fetch;
            Tags = tags;
        }

        public int Offset { get; set; }

        public int Fetch { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    public class GetImagesModel
        : IModel
    {
        public int Offset { get; set; }

        public int Fetch { get; set; }

        public int Count 
        {  
            get 
            {
                return Images.Count;
            } 
        }

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
            var model = new GetImagesModel()
            {
                Fetch = query.Fetch,
                Offset = query.Offset
            };

            if(query.Tags == null) 
            {
                LoadImages(model, query);
            } 
            else 
            {
                LoadImagesWithTags(model, query);
            }

            return Task.FromResult((IModel)model);
        }

        private void LoadImages(GetImagesModel model, GetImages query)
        {
            foreach (var image in _storage.List(query.Offset, query.Fetch))
            {
                model.Add(image);
            }
        }

        private void LoadImagesWithTags(GetImagesModel model, GetImages query) 
        {
            foreach (var image in _storage.List(
                query.Offset, 
                query.Fetch, 
                image => image.Tags.Contains(query.Tags.First()))
            )
            {
                model.Add(image);
            }
        }
    }
}
