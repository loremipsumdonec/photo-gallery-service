using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;
using PhotoGalleryServiceTest.Utility;
using System.Collections.Generic;

namespace PhotoGalleryServiceTest.Services
{
    public class GalleryFixture
    {
        private readonly PhotoGalleryServiceEngine _engine;

        public GalleryFixture(
            PhotoGalleryServiceEngine engine
        )
        {
            _engine = engine;
            _engine.Start();

            Clear();
        }

        private void Clear()
        {
            IAlbumStorage storage = GetService<IAlbumStorage>();
            storage.Clear();
        }

        public T GetService<T>()
        {
            return (T)_engine.Services.GetService(typeof(T));
        }

        public IEnumerable<Album> Gallery
        {
            get
            {
                IAlbumStorage storage = GetService<IAlbumStorage>();
                return storage.List(0, 100000);
            }
        }

        public Album GetAlbum(Album template)
        {
            IAlbumStorage storage = GetService<IAlbumStorage>();
            return storage.Get(template.AlbumId);
        }

        public GalleryFixture CreateAlbums(int total)
        {
            IAlbumStorage storage = GetService<IAlbumStorage>();

            for (int index = 0; index < total; index++)
            {
                storage.Create(template =>
                {
                    template.Name = IpsumGenerator.Generate(2, 5, false);
                });
            }

            return this;
        }
    }
}
