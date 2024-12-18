using Library.Cache;
using Library.Interfaces;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Repos
{
    public class FileDocumentRepository : IDocumentRepository
    {
        private readonly string _directoryPath;
        private readonly DocumentCache _cache;

        public FileDocumentRepository(string directoryPath, DocumentCache cache)
        {
            _directoryPath = directoryPath;
            _cache = cache;
        }

        public async Task<List<IDocument>> SearchByNumberAsync(string documentNumber)
        {
            var results = new List<IDocument>();

            // Consultar primero en la caché
            var cachedDocument = _cache.GetFromCache(documentNumber);
            if (cachedDocument != null)
            {
                results.Add(cachedDocument);
                return results;
            }

            // Leer archivos desde el sistema si no está en caché
            foreach (var filePath in Directory.GetFiles(_directoryPath, "*.json"))
            {
                var jsonContent = await File.ReadAllTextAsync(filePath);
                var type = Path.GetFileName(filePath).Split('_')[0];

                IDocument document = type.ToLower() switch
                {
                    "patent" => JsonSerializer.Deserialize<Patent>(jsonContent),
                    "book" => JsonSerializer.Deserialize<Book>(jsonContent),
                    "localizedbook" => JsonSerializer.Deserialize<LocalizedBook>(jsonContent),
                    "magazine" => JsonSerializer.Deserialize<Magazine>(jsonContent),
                    _ => null,
                };

                if (document != null && document.Number.Equals(documentNumber, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(document);
                    _cache.AddToCache(documentNumber, document, type); // Agregar a caché
                }
            }

            return results;
        }
    }
}
