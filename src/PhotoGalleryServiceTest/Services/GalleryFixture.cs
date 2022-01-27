using Autofac;
using Autofac.Core;
using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using MassTransit;
using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;
using PhotoGalleryServiceTest.Utility;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;

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

            //Clear();
        }

        public Resources Resources { get; } = new();

        private void Clear()
        {
            GetService<IAlbumStorage>().Clear();
            GetService<IImageStorage>().Clear();
            GetService<IImageFileStorage>().Clear();
        }

        public T GetService<T>()
        {
            return (T)_engine.Services.GetService(typeof(T));
        }

        public T GetService<T>(params object[] parameters)
        {
            var scope = (ILifetimeScope)_engine.Services.GetService(typeof(ILifetimeScope));

            List<Parameter> pp = new List<Parameter>();

            foreach(var parameter in parameters) 
            {
                pp.Add(new TypedParameter(parameter.GetType(), parameter));
            }

            return scope.Resolve<T>(pp);
        }

        public void DistributeEvent(IEvent @event) 
        {
            var endpoint = GetService<IPublishEndpoint>();
            endpoint.Publish(@event, @event.GetType());
        }

        public IEvent WaitForEvent(Type eventType, int timeout = 5000) 
        {
            IEvent @event = null;
            var set = new ManualResetEventSlim();

            var hub = GetService<IEventHub>();
            hub.Connect((stream) => 
                stream.Where(e => e.GetType() == eventType)
                    .Subscribe(e => {
                        @event = e;
                        set.Set();
                    })
            );

            set.Wait(timeout);

            if(@event == null) 
            {
                throw new TimeoutException($"Timed out when waiting for event {eventType}");
            }

            return @event;
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
                        image.Name = Guid.NewGuid().ToString("N");
                    });
                }
            }

            return this;
        }

        #endregion

        #region ImageFiles

        public List<string> GetImageFiles() 
        {
            return Resources.Get("Images");
        }

        public byte[] ReadAllBytesFromRandomImageFile() 
        {
            return Resources.ReadAllBytes(
                GetImageFiles().PickRandom()
            );
        }

        public byte[] GetImageFile(Image image) 
        {
            IImageFileStorage storage = GetService<IImageFileStorage>();
            return storage.Download(image.ImageId);
        }

        public GalleryFixture WithData() 
        {
            IImageFileStorage storage = GetService<IImageFileStorage>();

            foreach (var image in Images) 
            {
                var randomData = ReadAllBytesFromRandomImageFile();
                storage.Upload(image.ImageId, randomData);
            }

            return this;
        }

        #endregion
    }
}
