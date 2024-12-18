using Library.Cache;
using Library.Interfaces;
using Library.Repos;

namespace Library
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            

            var cacheConfig = new Dictionary<string, TimeSpan?>
            {
                { "patent", TimeSpan.FromMinutes(10) },
                { "book", TimeSpan.FromHours(1) },
                { "localizedbook", null },
                { "magazine", TimeSpan.FromMinutes(5) },
            };

            var cache = new DocumentCache(cacheConfig);

            try
            {
                IDocumentRepository repository = new FileDocumentRepository("documents", cache);

                while (true)
                {
                    Console.WriteLine("Enter document number to search (or type 'exit' to quit):");
                    string input = Console.ReadLine() ?? "";

                    if (input.ToLower() == "exit")
                    {
                        Console.WriteLine("Goodbye!");
                        break;
                    }

                    var documents = await repository.SearchByNumberAsync(input);

                    if (documents.Count > 0)
                    {
                        Console.WriteLine("Search results:");
                        foreach (var doc in documents)
                        {
                            Console.WriteLine(doc.GetCardInfo());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No documents found.");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
