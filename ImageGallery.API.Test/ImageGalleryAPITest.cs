using ImageGallery.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Xunit;

namespace ImageGallery.API.Test
{
    public class ImageGalleryAPITest
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public ImageGalleryAPITest()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public void ShouldGetGalleryContest()
        {
            // Act
            var context = server.Host.Services.GetService(typeof(GalleryContext));

            // Assert
            Assert.NotNull(context);
            Assert.IsType<GalleryContext>(context);
        }
    }
}
