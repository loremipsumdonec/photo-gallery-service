using PhotoGalleryService.Features.Gallery.Queries;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Schema
{
    public class GalleryQuery
    {
        public Task<GetImagesModel> Images(
            int offset, 
            int fetch, 
            [Service] IQueryDispatcher dispatcher, 
            IEnumerable<string> tags = null) 
        {
            return dispatcher.DispatchAsync<GetImagesModel>(new GetImages(offset, fetch, tags));
        }
    }
}
