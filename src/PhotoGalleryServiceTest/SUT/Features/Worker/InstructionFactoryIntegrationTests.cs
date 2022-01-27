using PhotoGalleryService.Features.Worker.Instructions;
using PhotoGalleryService.Features.Worker.Services;
using PhotoGalleryServiceTest.Services;
using System.Linq;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Worker
{
    [Collection("PhotoGalleryServiceEngineForSmoke"), Trait("type", "Smoke")]
    public class InstructionFactoryIntegrationTests
    {
        public InstructionFactoryIntegrationTests(PhotoGalleryServiceEngineForSmoke engine)
        {
            Fixture = new GalleryFixture(engine);
        }

        public GalleryFixture Fixture { get; set; }

        [Fact]
        public void Lorem() 
        {
            string instructionsAsString = "format(png);blur(4.5, 34.0);resize(640,480)";
            
            var factory = Fixture.GetService<IInstructionFactory>();
            var instructions = factory.Create(instructionsAsString);

            Assert.NotEmpty(instructions);
            Assert.Equal(typeof(Blur), instructions.First()?.GetType());
        }
    }
}
