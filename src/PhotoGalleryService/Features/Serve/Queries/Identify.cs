using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

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
            var model = new IdentifyModel()
            {
                MIMEType = "image/jpeg"
            };

            return Task.FromResult((IModel) model);
        }
    }
}
