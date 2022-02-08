
namespace PhotoGalleryService.Features.Magick.Instructions
{
    public interface IInstruction
    {
        public void Apply(IConvertContext context);
    }
}
