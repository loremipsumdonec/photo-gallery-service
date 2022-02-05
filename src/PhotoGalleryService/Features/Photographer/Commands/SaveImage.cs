using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using PhotoGalleryService.Features.Gallery.Commands;
using RemotePhotographer.Features.Photographer.Events;

namespace PhotoGalleryService.Features.Photographer.Commands
{
    public class SaveImage
        : Command
    {
        public SaveImage()
        {
        }

        public SaveImage(ImageCaptured @event) 
        {
            Data = @event.Data;

            if(@event.Tags != null) 
            {
                Tags = new List<string>(@event.Tags);
            }
        }

        public SaveImage(VideoImageCaptured @event)
        {
            Data = @event.Data;

            if (@event.Tags != null)
            {
                Tags = new List<string>(@event.Tags);
            }
        }

        public byte[] Data { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    [Handle(typeof(SaveImage))]
    public class SaveImageHandler
        : CommandHandler<SaveImage>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public SaveImageHandler(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public override async Task<bool> ExecuteAsync(SaveImage command)
        {
            string imageId = await CreateImageAsync(command);
            await UploadImageFile(command, imageId);

            return true;
        }

        private async Task<string> CreateImageAsync(SaveImage command) 
        {
            var createImage = new CreateImage(command.Tags);
            await _commandDispatcher.DispatchAsync(createImage);

            return (string)createImage.CommandResult.Output;
        }
    
        private Task UploadImageFile(SaveImage command, string imageId) 
        {
            return _commandDispatcher.DispatchAsync(
                new UploadImageFile(imageId, command.Data)
            );
        }
    }
}
