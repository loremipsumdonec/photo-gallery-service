using Boilerplate.Features.Core.Commands;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryService.Features.Gallery.Services;

namespace PhotoGalleryService.Features.Serve.Controllers
{
    [Route("/serve"), ApiController]
    public class ImagesController
        : ControllerBase
    {
        private readonly IImageFileStorage _storage;
        private readonly ICommandDispatcher _dispatcher;

        public ImagesController(IImageFileStorage storage, ICommandDispatcher dispatcher)
        {
            _storage = storage;
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("magick/images/{imageId}")]
        public async Task<object> GetImageWithMagick(
            [FromRoute] string imageId,
            [ModelBinder(typeof(Magick.Bindings.InstructionsModelBinder))] IEnumerable<Magick.Instructions.IInstruction> apply)
        {
            string mimeType = "image/jpg";

            var instructions = new List<Magick.Instructions.IInstruction>();
            instructions.AddRange(apply);

            var stream = _storage.Download(imageId);

            if (stream == null)
            {
                return NotFound($"Could not find image with id {imageId}");
            }

            var command = new Magick.Commands.ProcessImage(stream, instructions);
            await _dispatcher.DispatchAsync(command);

            return File((byte[])command.CommandResult.Output, mimeType);
        }

        [HttpGet, Route("imagesharp/images/{imageId}")]
        public async Task<object> GetImageWithImageSharp(
            [FromRoute] string imageId,
            [ModelBinder(typeof(ImageSharp.Bindings.InstructionsModelBinder))] IEnumerable<ImageSharp.Instructions.IInstruction> apply)
        {
            string mimeType = "image/jpg";

            var instructions = new List<ImageSharp.Instructions.IInstruction>();
            instructions.AddRange(apply);

            var stream = _storage.Download(imageId);

            if(stream == null)
            {
                return NotFound($"Could not find image with id {imageId}");
            }

            var command = new ImageSharp.Commands.ProcessImage(stream, instructions);
            await _dispatcher.DispatchAsync(command);

            return File((byte[])command.CommandResult.Output, mimeType);
        }
    }
}
