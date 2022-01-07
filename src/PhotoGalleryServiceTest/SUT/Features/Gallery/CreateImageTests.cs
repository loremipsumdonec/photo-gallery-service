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
    public class CreateImageTests
    {
        public CreateImageTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateImage_WithValidInput_ImageCreated()
        {
            Fixture.CreateAlbums(1);
            var album = Fixture.Albums.First();

            var command = new CreateImage(
                album.AlbumId,
                IpsumGenerator.Generate(2, 3, false),
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            Assert.Single(Fixture.Images);

            var image = Fixture.Images.First();

            Assert.Equal(command.AlbumId, image.AlbumId);
            Assert.Equal(command.Name, image.Name);
            Assert.Equal(command.Description, image.Description);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateImage_WithSameNameAsOtherImageInSameAlbum_ThrowsArgumentException()
        {
            Fixture.CreateAlbums(1)
                .WithImages(1);

            var command = new CreateImage(
                Fixture.Images.First().AlbumId,
                Fixture.Images.First().Name,
                IpsumGenerator.Generate(8, 12, false)
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(command)
            );
        }
    }
}
