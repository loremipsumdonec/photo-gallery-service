using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Services;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class DeleteAlbum
        : Command
    {
        public DeleteAlbum(string albumId)
        {
            AlbumId = albumId;
        }

        public string AlbumId { get; set; }
    }

    [Handle(typeof(DeleteAlbum))]
    public class DeleteTemplateHandler
        : CommandHandler<DeleteAlbum>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IAlbumStorage _storage;

        public DeleteTemplateHandler(IEventDispatcher dispatcher, IAlbumStorage storage)
        {
            _dispatcher = dispatcher;
            _storage = storage;
        }

        public override Task<bool> ExecuteAsync(DeleteAlbum command)
        {
            Validate(command);

            var album = _storage.Delete(command.AlbumId);

            _dispatcher.Dispatch(new AlbumDeleted(album));

            return Task.FromResult(true);
        }

        private void Validate(DeleteAlbum command)
        {
            ValidateThatTemplateExists(command);
        }

        private void ValidateThatTemplateExists(DeleteAlbum command)
        {
            if (_storage.Get(command.AlbumId) is null)
            {
                throw new ArgumentException($"album with id {command.AlbumId} does not exists");
            }
        }
    }
}
