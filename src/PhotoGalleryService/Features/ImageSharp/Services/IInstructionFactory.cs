using PhotoGalleryService.Features.ImageSharp.Instructions;

namespace PhotoGalleryService.Features.ImageSharp.Services
{
    public interface IInstructionFactory 
    {
        IEnumerable<IInstruction> Create(string instructions);
    }
}
