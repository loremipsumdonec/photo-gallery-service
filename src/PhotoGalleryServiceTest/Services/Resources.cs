using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoGalleryServiceTest.Services
{
    public class Resources
    {
        private readonly List<string> _files;
        private readonly Random _random;

        public Resources()
            : this(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources"))
        {
        }

        public Resources(string path)
        {
            Path = path;
            _files = new List<string>();
            _random = new Random();

            Load();
        }

        public string Path { get; }

        public FileInfo GetFileInfo(string file)
        {
            string path = $"{Path}{file}";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            return new FileInfo(path);
        }

        public byte[] ReadAllBytes(string file) 
        {
            return File.ReadAllBytes(System.IO.Path.Combine(Path, file));
        }

        public List<string> Get(string prefix, int max = 0)
        {
            var hits = _files.FindAll(f => f.ToLower().StartsWith(prefix.ToLower()));

            if (max > 0)
            {
                return hits.Take(max).ToList();
            }

            return hits.ToList();
        }

        public string GetRandom(string prefix)
        {
            prefix = prefix
                .ToLower();

            var hits = _files.FindAll(f => f.ToLower().StartsWith(prefix));
            return hits[_random.Next(0, hits.Count - 1)];
        }

        private void Load()
        {
            var files = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                _files.Add(
                    file
                    .Replace(Path, "")
                    .Substring(1)
                );
            }
        }
    }
}
