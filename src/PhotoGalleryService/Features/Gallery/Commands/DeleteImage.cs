using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class DeleteImage
        : Command
    {
        public DeleteImage(string imageId)
        {
            ImageId = imageId;
        }

        public string ImageId { get; set; }
    }

    [Handle(typeof(DeleteImage))]
    public class DeleteImageHandler
        : CommandHandler<DeleteImage>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IImageStorage _storage;

        public DeleteImageHandler(IEventDispatcher dispatcher, IImageStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<bool> ExecuteAsync(DeleteImage command)
        {
            Validate(command);

            var image = _storage.Delete(command.ImageId);

            _dispatcher.Dispatch(new ImageDeleted(image));

            return Task.FromResult(true);
        }

        private void Validate(DeleteImage command)
        {
            ValidateThatImageExists(command);
        }

        private void ValidateThatImageExists(DeleteImage command)
        {
            if (_storage.Get(command.ImageId) is null)
            {
                throw new ArgumentException($"album with id {command.ImageId} does not exists");
            }
        }
    }
}
