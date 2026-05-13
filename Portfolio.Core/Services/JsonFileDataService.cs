using System.Text.Json;

namespace Portfolio.Core.Services
{
    public class JsonFileDataService
    {
        private readonly string _filePath;

        public JsonFileDataService(string fileName = "data/cart_storage.json")
        {
            // Ensure the directory exists
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public async Task<List<T>> ReadAsync<T>(string filePath)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            if (!File.Exists(fullPath)) return new List<T>();
            
            using var stream = File.OpenRead(fullPath);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
        }

        public async Task WriteAsync<T>(List<T> data, string filePath)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            using var stream = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}