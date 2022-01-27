using PhotoGalleryService.Features.Worker.Instructions;

namespace PhotoGalleryService.Features.Worker.Services
{
    public interface IInstructionFactory 
    {
        IEnumerable<IInstruction> Create(string instructions);
    }
}
