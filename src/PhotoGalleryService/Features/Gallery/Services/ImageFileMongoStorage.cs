using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public class ImageFileMongoStorage
        : IImageFileStorage
    {
        private readonly IMongoDatabase _database;
        private readonly IGridFSBucket _bucket;

        public ImageFileMongoStorage(
            string hostname,
            int port,
            string credentialDatabaseName,
            string username,
            string password,
            string databaseName)
        {
            MongoClient client = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(hostname, port),
                Credential = MongoCredential.CreateCredential(credentialDatabaseName, username, password),
                ConnectTimeout = TimeSpan.FromSeconds(5),
            });

            _database = client.GetDatabase(databaseName);

            _bucket = new GridFSBucket(_database, new GridFSBucketOptions
            {
                BucketName = "images",
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });
        }

        public void Clear() 
        {
            _bucket.Drop();
        }

        public string Upload(string imageId, byte[] data) 
        {
            var id = _bucket.UploadFromBytes(imageId, data);
            return id.ToString();
        }

        public byte[] Download(string imageId) 
        {
            try 
            {
                return _bucket.DownloadAsBytesByName(imageId);
            }
            catch (GridFSFileNotFoundException) 
            {
                return null;
            }
        }

        public void Delete(string imageId) 
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, imageId);
            var file = _bucket.Find(filter).FirstOrDefault();

            if(file != null)
            {
                _bucket.Delete(file.Id);
            }
        }
    }
}
