using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using PhotoGalleryService.Features.Magick.Instructions;
using PhotoGalleryService.Features.Magick.Services;

namespace PhotoGalleryService.Features.Magick.Commands
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
            using(var converter = new MagickConverter("magick", System.IO.Path.GetTempPath())) 
            {
                foreach(var instruction in command.Instructions) 
                {
                    converter.Add(instruction);
                }

                return await converter.ConvertAsync(command.Image);
            }
        }
    }
}
