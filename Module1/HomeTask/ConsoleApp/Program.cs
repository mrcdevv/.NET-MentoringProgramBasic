using GreetingService;
public class Program
{
    private static void Main(string[] args)
    {

        Console.WriteLine("Please introduce your username: ");
        string username = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("\nYou should introduce a valid username: ");
            username = Console.ReadLine();
        }

        Console.WriteLine($"\n{new Service().HandleGreeting(username)}");
    }
}