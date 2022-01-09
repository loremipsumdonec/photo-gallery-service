using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Queries;
using RemotePhotographer.Features.Photographer.Events;

namespace PhotoGalleryService.Features.Photographer.Commands
{
    public class SaveImage
        : Command
    {
        public SaveImage(ImageCaptured @event) 
        {
            Path = @event.Path;
            Data = @event.Data;
        }

        public string Path { get; set; }

        public byte[] Data { get; set; }
    }

    [Handle(typeof(SaveImage))]
    public class SaveImageHandler
        : CommandHandler<SaveImage>
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public SaveImageHandler(
            ICommandDispatcher commandDispatcher, 
            IQueryDispatcher queryDispatcher
        )
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<bool> ExecuteAsync(SaveImage command)
        {
            var albumId = await GetOrCreateAlbumAsync(command);
            string imageId = await CreateImageAsync(command, albumId);
            await UploadImageFile(command, imageId);

            return true;
        }

        private async Task<string> GetOrCreateAlbumAsync(SaveImage command) 
        {
            string albumName = System.IO.Path.GetDirectoryName(command.Path);

            var album = await _queryDispatcher.DispatchAsync<Album>(
                new GetAlbum(albumName)
            );

            if(album != null)
            {
                return album.AlbumId;
            }

            var createAlbum = new CreateAlbum(albumName, string.Empty);
            await _commandDispatcher.DispatchAsync(createAlbum);

            album = await _queryDispatcher.DispatchAsync<Album>(
                new GetAlbum((string)createAlbum.CommandResult.Output)
            );

            return album.AlbumId;
        }
    
        private async Task<string> CreateImageAsync(SaveImage command, string albumId) 
        {
            string imageName = System.IO.Path.GetFileName(command.Path);

            var createImage = new CreateImage(albumId, imageName, "");
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
