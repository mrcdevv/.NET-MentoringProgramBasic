using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Select a task:");
            Console.WriteLine("1. Get my name");
            Console.WriteLine("2. Test HTTP status codes");
            Console.WriteLine("3. Get my name from header");
            Console.WriteLine("4. Get my name from cookies");
            Console.WriteLine("5. Exit");
            Console.Write("Option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await GetMyName();
                    break;
                case "2":
                    await TestHttpStatusCodes();
                    break;
                case "3":
                    await GetMyNameFromHeader();
                    break;
                case "4":
                    await GetMyNameFromCookies();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static async Task GetMyName()
    {
        string url = "http://localhost:8888/MyName/";
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string myName = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"My Name: {myName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMyName: {ex.Message}");
        }
    }

    private static async Task TestHttpStatusCodes()
    {
        string[] urls = {
            "http://localhost:8888/Information/",
            "http://localhost:8888/Success/",
            "http://localhost:8888/Redirection/",
            "http://localhost:8888/ClientError/",
            "http://localhost:8888/ServerError/"
        };

        foreach (var url in urls)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                Console.WriteLine($"URL: {url}, Status Code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TestHttpStatusCodes for URL {url}: {ex.Message}");
            }
        }
    }

    private static async Task GetMyNameFromHeader()
    {
        string url = "http://localhost:8888/MyNameByHeader/";
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.Headers.TryGetValues("X-MyName", out var values))
            {
                Console.WriteLine($"My Name from Header: {string.Join(", ", values)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMyNameFromHeader: {ex.Message}");
        }
    }

    private static async Task GetMyNameFromCookies()
    {
        string url = "http://localhost:8888/MyNameByCookies/";
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                foreach (var cookie in values)
                {
                    if (cookie.Contains("MyName"))
                    {
                        Console.WriteLine($"My Name from Cookies: {cookie.Split('=')[1]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMyNameFromCookies: {ex.Message}");
        }
    }
}