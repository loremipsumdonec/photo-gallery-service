using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class UpdateAlbum
        : Command
    {
        public UpdateAlbum(
            string albumId,
            string name,
            string description)
        {
            AlbumId = albumId;
            Name = name;
            Description = description;
        }

        public string AlbumId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    [Handle(typeof(UpdateAlbum))]
    public class UpdateTemplateHandler
        : CommandHandler<UpdateAlbum>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IAlbumStorage _storage;

        public UpdateTemplateHandler(IEventDispatcher dispatcher, IAlbumStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<bool> ExecuteAsync(UpdateAlbum command)
        {
            Validate(command);

            var album = _storage.Update(command.AlbumId, album =>
            {
                album.Name = command.Name;
                album.Description = command.Description;
            });

            _dispatcher.Dispatch(new AlbumUpdated(album));

            return Task.FromResult(true);
        }

        private void Validate(UpdateAlbum command)
        {
            ValidateThatTemplateExists(command);
            ValidateUniqueName(command);
        }

        private void ValidateThatTemplateExists(UpdateAlbum command)
        {
            if (_storage.Get(command.AlbumId) is null)
            {
                throw new ArgumentException($"album with id {command.AlbumId} does not exists");
            }
        }

        private void ValidateUniqueName(UpdateAlbum command)
        {
            if (_storage.List(0, 1, (template) => template.Name == command.Name).Any())
            {
                throw new ArgumentException($"album does not have a unique name");
            }
        }
    }
}
