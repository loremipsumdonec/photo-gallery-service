using Elastic.Apm.StackExchange.Redis;
using StackExchange.Redis;

namespace PhotoGalleryService.Features.Gallery.Services
{
    public class ImageFileRedisStorage
        : IImageFileStorage
    {
        private readonly ConnectionMultiplexer _redis;

        public ImageFileRedisStorage(string hostname, int port)
        {
            _redis = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                AllowAdmin = true,
                EndPoints = { $"{hostname}:{port}" },
                ConnectTimeout = 30000,
                SyncTimeout = 30000
            });

            _redis.UseElasticApm();
        }

        public void Clear()
        {
            var endPoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endPoints[0]);
            server.FlushAllDatabases();
        }

        public void Delete(string imageId)
        {
            var database = _redis.GetDatabase();
            database.KeyDelete(imageId);
        }

        public byte[] Download(string imageId)
        {
            var database = _redis.GetDatabase();
            var value = database.StringGet(imageId);

            return (byte[])value.Box();
        }

        public void Upload(string imageId, byte[] data)
        {
            var database = _redis.GetDatabase();
            database.StringSet(imageId, data);
        }
    }
}
