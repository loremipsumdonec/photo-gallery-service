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
            var command = new CreateImage(
                IpsumGenerator.Generate(2, 3, false),
                IpsumGenerator.Generate(5, 6, false),
                new string[] { "A", "B", "C"}
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();
            await dispatcher.DispatchAsync(command);

            Assert.Single(Fixture.Images);

            var image = Fixture.Images.First();

            Assert.Equal(command.Name, image.Name);
            Assert.Equal(command.Description, image.Description);
            Assert.Equal(command.Tags, image.Tags);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public async Task CreateImage_WithSameNameAsOtherImageInSameAlbum_ThrowsArgumentException()
        {
            Fixture.CreateImages(1);

            var command = new CreateImage(
                Fixture.Images.First().Name,
                Fixture.Images.First().Description,
                Fixture.Images.First().Tags
            );

            var dispatcher = Fixture.GetService<ICommandDispatcher>();

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await dispatcher.DispatchAsync(command)
            );
        }
    }
}
