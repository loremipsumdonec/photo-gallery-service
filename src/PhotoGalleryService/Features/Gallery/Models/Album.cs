using Boilerplate.Features.Core;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoGalleryService.Features.Gallery.Models
{
    public class Album
        : IModel
    {
        public Album(string name, string? description)
        {
            Name = name;
            Description = description;
        }

        public Album(Album? album = null)
        {
            if(album == null)
            {
                return;
            }

            AlbumId = album.AlbumId;
            Name = album.Name;
            Description = album.Description;
            IsDeleted = album.IsDeleted;
            Created = album.Created;
            Updated = album.Updated;
            Deleted = album.Deleted;
        }

        [BsonId]
        public string AlbumId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Deleted { get; set; }
    }
}
