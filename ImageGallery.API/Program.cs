using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ImageGallery.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitializeAutoMapper();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static void InitializeAutoMapper()
        {
            // map to model
            // AutoMapper
            // https://www.nuget.org/packages/AutoMapper
            // Install-Package -Id AutoMapper -Project ImageGallery.API
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Map from Image(entity) to Image(model), and back
                cfg.CreateMap<Entities.Image, Model.Image>().ReverseMap();

                // TODO do another setting
            });

            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
