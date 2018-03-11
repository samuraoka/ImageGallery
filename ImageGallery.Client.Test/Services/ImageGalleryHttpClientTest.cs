using ImageGallery.Client.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

//TODO refactor this test class name and method name.
namespace ImageGallery.Client.Test.Services
{
    public class ImageGalleryHttpClientTest
    {
        private HttpClient httpClient = new HttpClient();

        [Fact]
        public void ShouldBeAbleToGetHttpClient()
        {
            // Arrange
            var proxy = new ImageGalleryHttpClient();

            // Act
            var client = proxy.GetClient();

            // Assert
            Assert.IsType<HttpClient>(client);
            Assert.Equal("http://localhost:58828/", client.BaseAddress.ToString());
            Assert.Contains(new MediaTypeWithQualityHeaderValue("application/json"), client.DefaultRequestHeaders.Accept);
        }
    }
}
