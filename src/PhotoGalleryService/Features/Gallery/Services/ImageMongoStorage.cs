using PhotoGalleryService.Features.Gallery.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public class ImageMongoStorage
        : IImageStorage
    {
        private readonly IMongoCollection<Image> _images;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public ImageMongoStorage(
            string hostname,
            int port,
            string credentialDatabaseName,
            string username,
            string password,
            string databaseName,
            string collectionName)
        {
            _collectionName = collectionName;

            MongoClient client = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(hostname, port),
                Credential = MongoCredential.CreateCredential(credentialDatabaseName, username, password),
                ConnectTimeout = TimeSpan.FromSeconds(5),
            });

            _database = client.GetDatabase(databaseName);
            _images = _database.GetCollection<Image>(_collectionName);
        }

        public Image Create(Action<Image> action)
        {
            Image Image = new Image()
            {
                ImageId = ObjectId.GenerateNewId().ToString()
            };

            action.Invoke(Image);

            _images.InsertOne(Image);

            return Image;
        }

        public Image Delete(string ImageId)
        {
            var Image = Get(ImageId);
            Image.Deleted = DateTime.Now;
            Image.IsDeleted = true;

            _images.ReplaceOne(t => t.ImageId == ImageId, Image);

            return Image;
        }

        public Image Get(string ImageId)
        {
            return _images.Find(t => t.ImageId == ImageId).FirstOrDefault();
        }

        public List<Image> List(
            int offset,
            int fetch,
            Expression<Func<Image, bool>> filter = null)
        {
            if (filter is null)
            {
                filter = (t) => true;
            }

            Expression<Func<Image, bool>> filterWithMongoImage =
                Expression.Lambda<Func<Image, bool>>(filter.Body, filter.Parameters);

            return _images.AsQueryable()
                .Where(filterWithMongoImage)
                .Skip(offset)
                .Take(fetch)
                .ToList();
        }

        public Image Update(string ImageId, Action<Image> action)
        {
            var source = Get(ImageId);

            action.Invoke(source);

            source.Updated = DateTime.Now;

            _images.ReplaceOne(
                t => t.ImageId == ImageId, source
            );

            return source;
        }

        public void Clear()
        {
            _database.DropCollection(_collectionName);
        }

        public Image Create(Image Image)
        {
            var source = new Image();
            source.ImageId = ObjectId.GenerateNewId().ToString();
            Load(source, Image);

            _images.InsertOne(source);

            return source;
        }

        private void Load(Image target, Image source)
        {
            target.Name = source.Name;
            target.Description = source.Description;
        }
    }
}
