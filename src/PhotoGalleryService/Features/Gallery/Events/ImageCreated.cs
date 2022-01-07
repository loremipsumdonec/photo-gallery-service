﻿using PhotoGalleryService.Features.Gallery.Models;
using Boilerplate.Features.Reactive.Events;

namespace PhotoGalleryService.Features.Gallery.Events
{
    public class ImageCreated
        : Image, IEvent
    {
        public ImageCreated(Image source)
            : base(source)
        {
        }
    }
}
