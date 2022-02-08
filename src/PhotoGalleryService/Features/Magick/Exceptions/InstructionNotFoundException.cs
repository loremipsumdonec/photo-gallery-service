namespace PhotoGalleryService.Features.Magick.Exceptions
{
    public sealed class InstructionNotFoundException
        : Exception
    {
        public InstructionNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
