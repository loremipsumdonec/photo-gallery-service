using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using ImageMagick;

namespace PhotoGalleryService.Features.Serve.Queries
{
    public class Identify
        : Query
    {
        public Identify(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }
    }

    public class IdentifyModel 
        : IModel
    {
        public string MIMEType { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }

    [Handle(typeof(Identify))]
    public class IdentifyHandler
        : QueryHandler<Identify>
    {
        public override Task<IModel> ExecuteAsync(Identify query)
        {
            var info = new MagickImageInfo(query.Data);
            
            var model = new IdentifyModel()
            {
                Height = info.Height,
                Width = info.Width,
                MIMEType = GetMIMEType(info.Format)
            };

            return Task.FromResult((IModel) model);
        }

        private string GetMIMEType(MagickFormat format) => format switch
        {
            MagickFormat.Png => "image/png",
            MagickFormat.Jpeg => "image/jpeg",
            MagickFormat.Cr2 => "image/x-canon-cr2",
            _ => Enum.GetName(format)
        };
    }
}
