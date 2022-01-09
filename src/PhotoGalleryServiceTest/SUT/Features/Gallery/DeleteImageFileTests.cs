using Boilerplate.Features.Core.Commands;
using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryServiceTest.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Gallery
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class DeleteImageFileTests
    {
        public DeleteImageFileTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
            Resources = new Resources();
        }

        public GalleryFixture Fixture { get; set; }

        public Resources Resources { get; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task DeleteImageFile_WhenImageFileExists_FileDeleted() 
        {
            Fixture.CreateAlbums(1)
                .WithImages(1)
                .WithData();

            var image = Fixture.Images.First();
            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(
                new DeleteImageFile(image.ImageId)
            );

            byte[] uploadedData = Fixture.GetImageFile(image);
            Assert.Null(uploadedData);
        }
    }
}
