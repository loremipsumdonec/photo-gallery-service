namespace PhotoGalleryService.Features.ImageSharp.Exceptions
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
