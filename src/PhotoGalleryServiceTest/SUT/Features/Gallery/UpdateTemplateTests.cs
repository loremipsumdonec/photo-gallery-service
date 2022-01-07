using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryServiceTest.Services;
using Boilerplate.Features.Core.Commands;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Gallery
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class UpdateAlbumTests
    {
        public UpdateAlbumTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task UpdateAlbum_WithValidInput_AlbumUpdated()
        {
            Fixture.CreateAlbums(1);
            var source = Fixture.Gallery.First();

            var command = new UpdateAlbum(
                source.AlbumId,
                "Updated name",
                "Updated description"
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            var updated = Fixture.GetAlbum(source);

            Assert.Equal(command.Name, updated.Name);
            Assert.Equal(command.Description, updated.Description);
            Assert.True(updated.Updated > source.Updated);
        }
    }
}
