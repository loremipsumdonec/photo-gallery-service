using Boilerplate.Features.Core.Commands;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryService.Features.Gallery.Services;
using PhotoGalleryService.Features.Worker.Bindings;
using PhotoGalleryService.Features.Worker.Commands;
using PhotoGalleryService.Features.Worker.Instructions;

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

        [HttpGet, Route("images/{imageId}.{extension}")]
        public async Task<object> GetImage(
            [FromRoute] string imageId,
            [FromRoute] string extension,
            [ModelBinder(typeof(InstructionsModelBinder))] IEnumerable<IInstruction> apply)
        {
            string mimeType = "image/jpg";

            List<IInstruction> instructions = new List<IInstruction>();

            switch(extension) 
            {
                case "jpg":
                    instructions.Add(new Worker.Instructions.Convert("jpg"));
                    break;
                case "png":
                    instructions.Add(new Worker.Instructions.Convert("png"));
                    mimeType = "image/png";
                    break;
                case "webp":
                    instructions.Add(new Worker.Instructions.Convert("webp"));
                    mimeType = "image/webp";
                    break;
            }

            instructions.AddRange(apply);

            var stream = _storage.Download(imageId);

            if(stream == null)
            {
                return NotFound($"Could not find image with id {imageId}");
            }

            var command = new ProcessImage(stream, instructions);
            await _dispatcher.DispatchAsync(command);

            return File((byte[])command.CommandResult.Output, mimeType);
        }
    }
}
