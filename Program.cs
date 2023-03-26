using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace App
{
    public class Program
    {
        public static void Main()
        {
            CreateWebHostBuilder()
            .UseKestrel(options => options.AddServerHeader = false)
            .UseSockets()
            .Build()
            .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
    }
}
