using Boilerplate.Features.MassTransit;
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events
{
    public class VideoImageCaptured
        : Event, IConsumed
    {
        public VideoImageCaptured()
        {
        }

        public byte[] Data { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
