using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryServiceTest.Services;
using PhotoGalleryServiceTest.Utility;
using Boilerplate.Features.Core.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Gallery
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class CreateAlbumTests
    {
        public CreateAlbumTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateAlbum_WithValidInput_AlbumCreated()
        {
            var command = new CreateAlbum(
                IpsumGenerator.Generate(2, 3, false),
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            Assert.Single(Fixture.Gallery);

            var album = Fixture.Gallery.First();

            Assert.Equal(command.Name, album.Name);
            Assert.Equal(command.Description, album.Description);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateAlbum_WithSameNameAsOtherAlbum_ThrowsArgumentException()
        {
            Fixture.CreateAlbums(1);

            var command = new CreateAlbum(
                Fixture.Gallery.First().Name,
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(command)
            );
        }
    }
}
