using System;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // TODO: Implement the task here.

            while (true)
            {
                try
                {
                    Console.WriteLine("\nEnter a line of text (or type 'exit' to quit): ");
                    string input = Console.ReadLine();

                    if (input?.ToLower() == "exit")
                    {
                        break;
                    }

                    FirstCharacter(input);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    break;
                }
            }

            Console.WriteLine("Program finished.");
        }


        public static void FirstCharacter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be empty. Please next time enter a valid string.");

            Console.WriteLine($"The first character is: {input[0]}");
        }
    }
}