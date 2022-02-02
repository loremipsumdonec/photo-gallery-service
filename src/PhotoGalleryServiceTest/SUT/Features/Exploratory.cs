using PhotoGalleryServiceTest.Services;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Exploratory")]
    public class Exploratory
    {
        public Exploratory(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        public void CreateImage_WithValidInput_ImageCreated()
        {
            Fixture.CreateImages(3, "primary", "lorem")
                .CreateImages(2, "seconday")
                .WithData();
        }
    }
}
