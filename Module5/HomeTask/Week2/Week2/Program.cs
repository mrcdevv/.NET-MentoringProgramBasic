using System;
using System.Threading.Tasks;

namespace Week2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new SampleConfiguration();

            await config.LoadSettingsAsync();
            Console.WriteLine($"MaxItems: {config.MaxItems}");
            Console.WriteLine($"Timeout: {config.Timeout}");
            Console.WriteLine($"AppName: {config.AppName}");

            config.MaxItems += 100;
            config.Timeout += " 30";
            config.AppName = "MyApplication";

            await config.SaveSettingsAsync();
            Console.WriteLine("Settings have been saved.");
        }
    }
}
