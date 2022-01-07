using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;
using PhotoGalleryServiceTest.Utility;
using System;
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
            GetService<IAlbumStorage>().Clear();
            GetService<IImageStorage>().Clear();
        }

        public T GetService<T>()
        {
            return (T)_engine.Services.GetService(typeof(T));
        }

        #region Albums

        public IEnumerable<Album> Albums
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

        #endregion

        #region Images

        public IEnumerable<Image> Images
        {
            get
            {
                IImageStorage storage = GetService<IImageStorage>();
                return storage.List(0, 100000);
            }
        }

        public Image GetImage(Image image)
        {
            IImageStorage storage = GetService<IImageStorage>();
            return storage.Get(image.ImageId);
        }

        public GalleryFixture WithImages(int total)
        {
            IImageStorage storage = GetService<IImageStorage>();

            foreach(var album in Albums) 
            {
                for (int index = 0; index < total; index++)
                {
                    storage.Create(image =>
                    {
                        image.AlbumId = album.AlbumId;
                        image.Name = Guid.NewGuid().ToString("N");
                    });
                }
            }

            return this;
        }

        #endregion
    }
}
