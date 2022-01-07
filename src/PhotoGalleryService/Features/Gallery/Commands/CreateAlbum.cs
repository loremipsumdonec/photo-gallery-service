using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class CreateAlbum
        : Command
    {
        public CreateAlbum(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    [Handle(typeof(CreateAlbum))]
    public class CreateTemplateHandler
        : CommandHandlerWithOutput<CreateAlbum, string>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IAlbumStorage _storage;

        public CreateTemplateHandler(IEventDispatcher dispatcher, IAlbumStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<string> ExecuteWithOutputAsync(CreateAlbum command)
        {
            Validate(command);

            var album = _storage.Create(album =>
            {
                album.Name = command.Name;
                album.Description = command.Description;
            });

            _dispatcher.Dispatch(new AlbumCreated(album));

            return Task.FromResult(album.AlbumId);
        }

        private void Validate(CreateAlbum command)
        {
            ValidateUniqueName(command);
        }

        private void ValidateUniqueName(CreateAlbum command)
        {
            if (_storage.List(0, 1, (template) => template.Name == command.Name).Any())
            {
                throw new ArgumentException($"album does not have a unique name");
            }
        }
    }
}
