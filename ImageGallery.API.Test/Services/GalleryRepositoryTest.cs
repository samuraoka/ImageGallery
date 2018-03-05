using ImageGallery.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Xunit;

namespace ImageGallery.API.Test.Services
{
    public class GalleryRepositoryTest
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public GalleryRepositoryTest()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public void ShouldGetGalleryRepository()
        {
            // Act
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository));

            // Assert
            Assert.NotNull(repo);
            Assert.IsAssignableFrom<IGalleryRepository>(repo);
        }
    }
}
