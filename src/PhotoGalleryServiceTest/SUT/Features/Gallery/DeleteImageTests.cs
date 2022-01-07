using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryServiceTest.Services;
using PhotoGalleryServiceTest.Utility;
using Boilerplate.Features.Core.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Gallery
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class DeleteImageTests
    {
        public DeleteImageTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task DeleteImage_ImageExists_ImageDeleted()
        {
            Fixture.CreateAlbums(1).WithImages(1);
            var image = Fixture.Images.PickRandom();
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await dispatcher.DispatchAsync(new DeleteImage(image.ImageId));

            var deleted = Fixture.GetImage(image);

            Assert.True(deleted.IsDeleted);
            Assert.True(deleted.Deleted > DateTime.MinValue);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void DeleteImage_WhenImageDoesNotExists_ThrowsException()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(new DeleteImage(Guid.NewGuid().ToString()))
            );
        }
    }
}
