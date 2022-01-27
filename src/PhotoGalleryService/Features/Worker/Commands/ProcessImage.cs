using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using ImageMagick;
using PhotoGalleryService.Features.Worker.Instructions;

namespace PhotoGalleryService.Features.Worker.Commands
{
    public class ProcessImage
        : Command
    {
        public ProcessImage(byte[] image, IEnumerable<IInstruction> instructions)
        {
            Image = image;
            Instructions = instructions;
        }

        public byte[] Image { get; set; }

        public IEnumerable<IInstruction> Instructions { get; set; }
    }

    [Handle(typeof(ProcessImage))]
    public class ProcessImageHandler
        : CommandHandlerWithOutput<ProcessImage, byte[]>
    {
        public override async Task<byte[]> ExecuteWithOutputAsync(ProcessImage command)
        {
            using (var stream = new MemoryStream())
            {
                using (var image = new MagickImage(command.Image))
                {
                    foreach (var instruction in command.Instructions)
                    {
                        await instruction.ApplyAsync(image);
                    }

                    await image.WriteAsync(stream);
                }

                return stream.ToArray();
            }
        }
    }
}
