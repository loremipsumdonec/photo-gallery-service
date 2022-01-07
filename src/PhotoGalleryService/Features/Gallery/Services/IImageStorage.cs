using PhotoGalleryService.Features.Gallery.Models;
using System.Linq.Expressions;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public interface IImageStorage
    {
        void Clear();

        Image Delete(string imageId);

        Image Create(Action<Image> action);

        Image Update(string imageId, Action<Image> action);

        Image Get(string imageId);

        List<Image> List(int offset, int fetch, Expression<Func<Image, bool>> filter = null);
    }
}
