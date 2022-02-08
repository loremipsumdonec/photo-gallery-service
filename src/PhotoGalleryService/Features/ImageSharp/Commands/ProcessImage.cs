using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using PhotoGalleryService.Features.ImageSharp.Instructions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace PhotoGalleryService.Features.ImageSharp.Commands
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
                using (var image = Image.Load(command.Image, out IImageFormat format))
                {
                    foreach (var instruction in command.Instructions)
                    {
                        await instruction.ApplyAsync(image);
                    }

                    await image.SaveAsJpegAsync(stream);
                }

                return stream.ToArray();
            }
        }
    }
}
