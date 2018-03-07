using ImageGallery.API.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ImageGallery.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AutoMapperHelper.InitializeAutoMapper();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
