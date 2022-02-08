namespace PhotoGalleryService.Features.Magick.Instructions
{
    public interface IConvertContext
    {
        public string In { get; }

        public string Out { get; }

        public string Format { get; set; }

        public void Append(string argument);

        public string Arguments { get; }
    }
}
