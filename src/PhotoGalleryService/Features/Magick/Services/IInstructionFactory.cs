using PhotoGalleryService.Features.Magick.Instructions;

namespace PhotoGalleryService.Features.Magick.Services
{
    public interface IInstructionFactory 
    {
        IEnumerable<IInstruction> Create(string instructions);
    }
}
