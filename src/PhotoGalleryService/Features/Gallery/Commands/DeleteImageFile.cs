using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class DeleteImageFile
        : Command
    {
        public DeleteImageFile(string imageId)
        {
            ImageId = imageId;
        }

        public string ImageId { get; set; }
    }

    [Handle(typeof(DeleteImageFile))]
    public class DeleteImageFileHandler
    : CommandHandler<DeleteImageFile>
    {
        private readonly IImageStorage _storage;
        private readonly IImageFileStorage _fileStorage;
        private readonly IEventDispatcher _dispatcher;

        public DeleteImageFileHandler(
            IImageStorage storage,
            IImageFileStorage fileStorage,
            IEventDispatcher dispatcher)
        {
            _storage = storage;
            _fileStorage = fileStorage;
            _dispatcher = dispatcher;
        }

        public override Task<bool> ExecuteAsync(DeleteImageFile command)
        {
            var image = _storage.Get(command.ImageId);

            if (image == null)
            {
                throw new ImageNotFoundException($"Could not find image with id {command.ImageId}");
            }

            _fileStorage.Delete(image.ImageId);
            _dispatcher.Dispatch(new ImageFileDeleted(image));

            return Task.FromResult(true);
        }
    }
}
