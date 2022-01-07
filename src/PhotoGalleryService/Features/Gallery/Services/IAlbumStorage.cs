using PhotoGalleryService.Features.Gallery.Models;
using System.Linq.Expressions;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public interface IAlbumStorage
    {
        void Clear();

        Album Delete(string templateId);

        Album Create(Action<Album> action);

        Album Update(string templateId, Action<Album> action);

        Album Get(string templateId);

        List<Album> List(int offset, int fetch, Expression<Func<Album, bool>>? filter = null);
    }
}
