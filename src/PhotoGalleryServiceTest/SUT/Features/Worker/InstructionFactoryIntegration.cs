using PhotoGalleryService.Features.Worker.Services;
using System.Collections.Generic;
using Xunit;

namespace PhotoGalleryServiceTest.SUT.Features.Worker
{
    [Trait("type", "Unit")]
    public class InstructionFactoryTests
    {
        [Theory]
        [InlineData("blur", "blur")]
        [InlineData("blur()", "blur")]
        [Trait("severity", "Critical")]
        public void GetName_HasExpected(string instruction, string expected)
        {
            var factory = new DefaultInstructionFactory();
            string name = factory.GetName(instruction);

            Assert.Equal(expected, name);
        }

        [Theory]
        [InlineData("blur", new object[] {})]
        [InlineData("blur()", new object[] { })]
        [InlineData("blur(10)", new object[] { 10 })]
        [InlineData("blur(11.332)", new object[] { 11.332 })]
        [InlineData("blur(10, 45)", new object[] { 10, 45 })]
        [InlineData("blur(true)", new object[] { true })]
        [InlineData("blur(false)", new object[] { false })]
        [InlineData("blur(lorem donec ipsum)", new object[] { "lorem donec ipsum" })]
        [Trait("severity", "Critical")]
        public void GetParameters_HasExpected(string instruction, IEnumerable<object> expected)
        {
            var factory = new DefaultInstructionFactory();
            var parameters = factory.GetParameters(instruction);

            Assert.Equal(expected, parameters);
        }

        /*
        [Fact]
        public void Lorem() 
        {
            string instructionsAsString = "blur(4.5, 34)";
            var registry = new DefaultInstructionRegistry();
            registry.Add("blur", "b", typeof(Blur));

            var factory = new InstructionFactory(registry);

            var instructions = factory.Create(instructionsAsString);

            Assert.NotEmpty(instructions);
            Assert.Equal(typeof(Blur), instructions.First()?.GetType());
        }
        */
    }
}
