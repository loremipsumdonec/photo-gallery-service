using Boilerplate.Features.Core;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoGalleryService.Features.Gallery.Models
{
    public class Image
        : IModel
    {
        public Image(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Image(Image image = null)
        {
            if (image == null)
            {
                return;
            }

            ImageId = image.ImageId;
            AlbumId = image.AlbumId;
            Name = image.Name;
            Description = image.Description;
            IsDeleted = image.IsDeleted;
            Created = image.Created;
            Updated = image.Updated;
            Deleted = image.Deleted;
        }

        [BsonId]
        public string ImageId { get; set; }

        public string AlbumId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Deleted { get; set; }
    }
}
