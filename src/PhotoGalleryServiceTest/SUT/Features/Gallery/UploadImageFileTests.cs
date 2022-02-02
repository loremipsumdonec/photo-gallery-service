using Boilerplate.Features.Core.Commands;
using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryServiceTest.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Gallery
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class UploadImageFileTests
    {
        public UploadImageFileTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task UploadImageFile_WithValidFile_FileUploaded() 
        {
            Fixture.CreateImages(1);

            var image = Fixture.Images.First();
            byte[] expectedData = Fixture.ReadAllBytesFromRandomImageFile();

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(
                new UploadImageFile(image.ImageId, expectedData)
            );

            byte[] uploadedData = Fixture.GetImageFile(image);
            Assert.Equal(expectedData, uploadedData);
        }
    }
}
