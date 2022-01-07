using PhotoGalleryService.Features.Gallery.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public class AlbumMongoStorage
        : IAlbumStorage
    {
        private readonly IMongoCollection<Album> _albums;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public AlbumMongoStorage(
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
            _albums = _database.GetCollection<Album>(_collectionName);
        }

        public Album Create(Action<Album> action)
        {
            Album album = new Album()
            {
                AlbumId = ObjectId.GenerateNewId().ToString()
            };

            action.Invoke(album);

            _albums.InsertOne(album);

            return album;
        }

        public Album Delete(string albumId)
        {
            var album = Get(albumId);
            album.Deleted = DateTime.Now;
            album.IsDeleted = true;

            _albums.ReplaceOne(t => t.AlbumId == albumId, album);

            return album;
        }

        public Album Get(string albumId)
        {
            return _albums.Find(t => t.AlbumId == albumId).FirstOrDefault();
        }

        public List<Album> List(
            int offset,
            int fetch,
            Expression<Func<Album, bool>>? filter = null)
        {
            if (filter is null)
            {
                filter = (t) => true;
            }

            Expression<Func<Album, bool>> filterWithMongoAlbum =
                Expression.Lambda<Func<Album, bool>>(filter.Body, filter.Parameters);

            return _albums.AsQueryable()
                .Where(filterWithMongoAlbum)
                .Skip(offset)
                .Take(fetch)
                .ToList();
        }

        public Album Update(string albumId, Action<Album> action)
        {
            var source = Get(albumId);

            action.Invoke(source);

            source.Updated = DateTime.Now;

            _albums.ReplaceOne(
                t => t.AlbumId == albumId, source
            );

            return source;
        }

        public void Clear()
        {
            _database.DropCollection(_collectionName);
        }

        public Album Create(Album album)
        {
            var source = new Album();
            source.AlbumId = ObjectId.GenerateNewId().ToString();
            Load(source, album);

            _albums.InsertOne(source);

            return source;
        }

        private void Load(Album target, Album source)
        {
            target.Name = source.Name;
            target.Description = source.Description;
        }
    }
}
