using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static HttpListener listener;
    private static bool running = true;

    static async Task Main(string[] args)
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/");
        listener.Start();
        Console.WriteLine("Listener started...");

        while (running)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                Console.WriteLine($"Request received: {context.Request.Url}");
                await ProcessRequestAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Listener: {ex.Message}");
            }
        }

        listener.Stop();
    }

    private static async Task ProcessRequestAsync(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        string resourcePath = request.Url.Segments[1].TrimEnd('/');

        Console.WriteLine($"Processing resource: {resourcePath}");

        switch (resourcePath)
        {
            case "MyName":
                await GetMyName(response);
                break;
            case "Information":
                await SendStatusAsync(response, HttpStatusCode.OK); // This first one isn't working if I put a Information status code. I dont know how make it work
                break;
            case "Success":
                await SendStatusAsync(response, HttpStatusCode.OK);
                break;
            case "Redirection":
                await SendStatusAsync(response, HttpStatusCode.Redirect);
                break;
            case "ClientError":
                await SendStatusAsync(response, HttpStatusCode.BadRequest);
                break;
            case "ServerError":
                await SendStatusAsync(response, HttpStatusCode.InternalServerError);
                break;
            case "MyNameByHeader":
                await GetMyNameByHeader(response);
                break;
            case "MyNameByCookies":
                await GetMyNameByCookies(response);
                break;
            case "Exit":
                running = false;
                response.StatusCode = (int)HttpStatusCode.OK;
                await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Server stopped."), 0, 0);
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                string notFoundMessage = "Resource not found.";
                byte[] notFoundBuffer = Encoding.UTF8.GetBytes(notFoundMessage);
                response.ContentLength64 = notFoundBuffer.Length;
                await response.OutputStream.WriteAsync(notFoundBuffer, 0, notFoundBuffer.Length);
                break;
        }

        response.Close();
    }

    private static async Task GetMyName(HttpListenerResponse response)
    {
        string myName = "Marco";
        byte[] buffer = Encoding.UTF8.GetBytes(myName);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }

    private static async Task SendStatusAsync(HttpListenerResponse response, HttpStatusCode statusCode)
    {
        response.StatusCode = (int)statusCode;
        string statusMessage = $"Status Code: {statusCode}";
        byte[] buffer = Encoding.UTF8.GetBytes(statusMessage);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }

    private static async Task GetMyNameByHeader(HttpListenerResponse response)
    {
        response.Headers.Add("X-MyName", "Marco");
        response.StatusCode = (int)HttpStatusCode.OK;
        await Task.CompletedTask;
    }

    private static async Task GetMyNameByCookies(HttpListenerResponse response)
    {
        response.Cookies.Add(new Cookie("MyName", "Marco"));
        response.StatusCode = (int)HttpStatusCode.OK;
        await Task.CompletedTask;
    }
}