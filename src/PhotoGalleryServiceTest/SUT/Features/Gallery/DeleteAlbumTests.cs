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
    public class DeleteAlbumTests
    {
        public DeleteAlbumTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task DeleteAlbum_AlbumExists_AlbumDeleted()
        {
            Fixture.CreateAlbums(1);
            var album = Fixture.Gallery.PickRandom();
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await dispatcher.DispatchAsync(new DeleteAlbum(album.AlbumId));

            var deleted = Fixture.GetAlbum(album);

            Assert.True(deleted.IsDeleted);
            Assert.True(deleted.Deleted > DateTime.MinValue);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void DeleteAlbum_WhenAlbumDoesNotExists_ThrowsException()
        {
            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(new DeleteAlbum(Guid.NewGuid().ToString()))
            );
        }
    }
}
