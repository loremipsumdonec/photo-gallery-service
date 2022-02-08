namespace PhotoGalleryService.Features.Magick.Services
{
    public interface IInstructionRegistry 
    {
        void Add(string name, string shortName, Type type);

        Type GetInstruction(string name);
    }
}
