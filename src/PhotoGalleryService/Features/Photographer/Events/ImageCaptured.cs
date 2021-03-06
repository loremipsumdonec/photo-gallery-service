using Boilerplate.Features.MassTransit;
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events
{
    public class ImageCaptured
        : Event, IConsumed
    {
        public ImageCaptured()
        {
        }

        public ImageCaptured(string path)
        {
            Path = path;
        }

        public ImageCaptured(string path, byte[] data)
        {
            Path = path;
            Data = data;
        }

        public string Path { get; set; }

        public byte[] Data { get; set; }

        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}
