using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryServiceTest.Services;
using PhotoGalleryServiceTest.Utility;
using RemotePhotographer.Features.Photographer.Events;
using System.Linq;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Photographer
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class PhotographerTests
    {
        public PhotographerTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
            Resources = new Resources();
        }

        public GalleryFixture Fixture { get; }

        public Resources Resources { get; }

        [Fact]
        public void ImageCaptured_ImageCreated() 
        {
            var image = Resources.Get("Images").PickRandom();

            ImageCaptured @event = new ImageCaptured(
                image,
                Resources.ReadAllBytes(image)
            );

            Fixture.DistributeEvent(@event);
            Fixture.WaitForEvent(typeof(ImageFileUploaded));

            Assert.Single(Fixture.Images);
            Assert.Equal(@event.Data, Fixture.GetImageFile(Fixture.Images.First()));
        }
    }
}
