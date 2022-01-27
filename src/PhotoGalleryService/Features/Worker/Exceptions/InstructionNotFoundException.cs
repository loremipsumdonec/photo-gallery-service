namespace PhotoGalleryService.Features.Worker.Exceptions
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
