namespace HomeTask
{
    class Program
    {
        private const string PATH = "/";

        static void Main(string[] args)
        {
            //Remember to change the PATH constant

            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. List all files and directories");
                Console.WriteLine("2. List filtered files and directories");
                Console.WriteLine("3. Exit");
                Console.Write("Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListAllFilesAndDirectories();
                        break;
                    case "2":
                        ListFilteredFilesAndDirectories();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void ListAllFilesAndDirectories()
        {
            var visitor = new FileSystemVisitor(PATH);

            visitor.Start += (sender, e) => Console.WriteLine("\nSearch started.");
            visitor.Finish += (sender, e) => Console.WriteLine("Search finished.\n");
            visitor.FileFound += (sender, e) => Console.WriteLine($"File found: {e.Path}");
            visitor.DirectoryFound += (sender, e) => Console.WriteLine($"Directory found: {e.Path}");

            foreach (var path in visitor.Read())
            {
            }
        }

        // This will show only the name of the files/folders that pass the filtering. Otherwise, you will only see a bunch of "File found" and "Directory found" 
        static void ListFilteredFilesAndDirectories()
        {
            var visitor = new FileSystemVisitor(PATH, path => path.EndsWith(".txt"));

            visitor.Start += (sender, e) => Console.WriteLine("\nSearch started.");
            visitor.Finish += (sender, e) => Console.WriteLine("Search finished.\n");
            visitor.FileFound += (sender, e) => Console.WriteLine("File found");
            visitor.DirectoryFound += (sender, e) => Console.WriteLine("Directory found");
            visitor.FilteredFileFound += (sender, e) => Console.WriteLine($"Filtered file found: {e.Path}");
            visitor.FilteredDirectoryFound += (sender, e) => Console.WriteLine($"Filtered directory found: {e.Path}");

            foreach (var path in visitor.Read())
            {
            }
        }
    }
}
