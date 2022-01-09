using Autofac;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Events;
using PhotoGalleryService.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Events;
using System.Reactive.Linq;

namespace PhotoGalleryService.Features.Photographer.Events
{
    public class PhotographerEventHandler
        : Boilerplate.Features.Reactive.Events.EventHandler
    {
        private readonly ILifetimeScope _scope;

        public PhotographerEventHandler(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public override void Connect(IObservable<IEvent> stream)
        {
            Disposables.Add(
                stream.Where(e => e is ImageCaptured)
                .Select(e => (ImageCaptured)e)
                .Select(e => Observable.FromAsync(async () => await OnImageCaptured(e)))
                .Concat()
                .Subscribe()
            );
        }

        public Task OnImageCaptured(ImageCaptured @event) 
        {
            var dispatcher = _scope.Resolve<ICommandDispatcher>();
            return dispatcher.DispatchAsync(new SaveImage(@event));
        }
    }
}
