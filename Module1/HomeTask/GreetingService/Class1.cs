using System;

namespace GreetingService
{
    public class Service
    {
        public string HandleGreeting(string username)
        {
            return $"{DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss")} Hello, {username}";
        }
    }
}
