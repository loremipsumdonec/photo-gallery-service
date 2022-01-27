using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class CreateImage
        : Command
    {
        public CreateImage(IEnumerable<string> tags)
        {
            Name = Guid.NewGuid().ToString("N");
            Tags = new List<string>(tags);
        }

        public CreateImage(string name, string description, IEnumerable<string> tags)
        {
            Name = name;
            Description = description;
            Tags = new List<string>(tags);
        }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }

    [Handle(typeof(CreateImage))]
    public class CreateImageHandler
        : CommandHandlerWithOutput<CreateImage, string>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IImageStorage _storage;

        public CreateImageHandler(IEventDispatcher dispatcher, IImageStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<string> ExecuteWithOutputAsync(CreateImage command)
        {
            Validate(command);

            var image = _storage.Create(image =>
            {
                image.Name = command.Name;
                image.Description = command.Description;
                image.Tags = new List<string>(command.Tags);
            });

            _dispatcher.Dispatch(new ImageCreated(image));

            return Task.FromResult(image.ImageId);
        }

        private void Validate(CreateImage command)
        {
            ValidateUniqueName(command);
        }

        private void ValidateUniqueName(CreateImage command)
        {
            if (_storage.List(0, 1, (image) => image.Name == command.Name).Any())
            {
                throw new ArgumentException($"image does not have a unique name");
            }
        }
    }
}
