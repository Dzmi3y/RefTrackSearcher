using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace RefTrackSearcher.Desktop.Utils
{
    public class UrlToBitmapConverter : IValueConverter
    {
        public static UrlToBitmapConverter Instance { get; } = new();
        private static readonly ConcurrentDictionary<string, Bitmap> _cache = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not string url || string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("URL is null or empty");
                return null;
            }

            Console.WriteLine($"Converting URL: {url}");

            if (_cache.TryGetValue(url, out var cachedImage))
            {
                Console.WriteLine("Returning cached image");
                return cachedImage;
            }

            try
            {
                Console.WriteLine($"Starting download from: {url}");
                
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                var bytes = client.GetByteArrayAsync(url).GetAwaiter().GetResult();
                Console.WriteLine($"Downloaded {bytes.Length} bytes");

                if (bytes.Length == 0)
                {
                    Console.WriteLine("Empty response");
                    return null;
                }
                
                if (bytes.Length > 10)
                {
                    Console.WriteLine($"First bytes: {BitConverter.ToString(bytes, 0, Math.Min(10, bytes.Length))}");
                }

                using var stream = new MemoryStream(bytes);
                var bitmap = new Bitmap(stream);

                _cache[url] = bitmap;
                Console.WriteLine("Image loaded successfully");
                return bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
