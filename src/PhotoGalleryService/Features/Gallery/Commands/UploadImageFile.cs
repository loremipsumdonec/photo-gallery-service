﻿using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using PhotoGalleryService.Features.Gallery.Events;
using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Services;

namespace PhotoGalleryService.Features.Gallery.Commands
{
    public class UploadImageFile
        : Command
    {
        public UploadImageFile(string imageId, byte[] data)
        {
            ImageId = imageId;
            Data = data;
        }

        public string ImageId { get; set; }

        public byte[] Data { get; set; }
    }

    [Handle(typeof(UploadImageFile))]
    public class UploadImageFileHandler
        : CommandHandler<UploadImageFile>
    {
        private readonly IImageStorage _storage;
        private readonly IImageFileStorage _fileStorage;
        private readonly IEventDispatcher _dispatcher;

        public UploadImageFileHandler(
            IImageStorage storage, 
            IImageFileStorage fileStorage, 
            IEventDispatcher dispatcher)
        {
            _storage = storage;
            _fileStorage = fileStorage;
            _dispatcher = dispatcher;
        }

        public override Task<bool> ExecuteAsync(UploadImageFile command)
        {
            var image = _storage.Get(command.ImageId);

            if(image == null) 
            {
                throw new ImageNotFoundException($"Could not find image with id {command.ImageId}");
            }

            _fileStorage.Upload(image.ImageId, command.Data);

            _dispatcher.Dispatch(new ImageFileUploaded(image));

            return Task.FromResult(true);
        }
    }
}
