using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Cache
{
    public class DocumentCache
    {
        private readonly Dictionary<string, (IDocument Document, DateTime Expiration)> _cache = new();
        private readonly Dictionary<string, TimeSpan?> _cacheConfig;

        public DocumentCache(Dictionary<string, TimeSpan?> cacheConfig)
        {
            _cacheConfig = cacheConfig;
        }

        public void AddToCache(string documentNumber, IDocument document, string documentType)
        {
            if (!_cacheConfig.TryGetValue(documentType.ToLower(), out var expirationTime) || expirationTime == null)
                return;

            var expiration = DateTime.UtcNow.Add(expirationTime.Value);
            _cache[documentNumber] = (document, expiration);
        }

        public IDocument GetFromCache(string documentNumber)
        {
            if (_cache.TryGetValue(documentNumber, out var entry) && entry.Expiration > DateTime.UtcNow)
                return entry.Document;

            _cache.Remove(documentNumber); // Remove expired entries
            return null;
        }
    }

}
