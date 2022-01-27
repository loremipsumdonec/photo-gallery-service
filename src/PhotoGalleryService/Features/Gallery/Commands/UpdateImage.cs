using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class UpdateImage
        : Command
    {
        public UpdateImage(
            string imageId,
            string name,
            string description)
        {
            ImageId = imageId;
            Name = name;
            Description = description;
        }

        public string ImageId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    [Handle(typeof(UpdateImage))]
    public class UpdateImageHandler
        : CommandHandler<UpdateImage>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IImageStorage _storage;

        public UpdateImageHandler(IEventDispatcher dispatcher, IImageStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<bool> ExecuteAsync(UpdateImage command)
        {
            Validate(command);

            var image = _storage.Update(command.ImageId, image =>
            {
                image.Name = command.Name;
                image.Description = command.Description;
                image.Updated = DateTime.Now;
            });

            _dispatcher.Dispatch(new ImageUpdated(image));

            return Task.FromResult(true);
        }

        private void Validate(UpdateImage command)
        {
            ValidateThatImageExists(command);
            ValidateUniqueName(command);
        }

        private void ValidateThatImageExists(UpdateImage command)
        {
            if (_storage.Get(command.ImageId) is null)
            {
                throw new ArgumentException($"image with id {command.ImageId} does not exists");
            }
        }

        private void ValidateUniqueName(UpdateImage command)
        {
            if (_storage.List(0, 1, (image) => image.Name == command.Name).Any())
            {
                throw new ArgumentException($"image does not have a unique name");
            }
        }
    }
}
