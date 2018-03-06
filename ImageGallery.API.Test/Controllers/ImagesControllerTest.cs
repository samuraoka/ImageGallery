using ImageGallery.API.Test.Fixture;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ImageGallery.API.Test.Controllers
{
    [Collection("Automapper collection")]
    public class ImagesControllerTest : IClassFixture<WebServerFixture>
    {
        private readonly TestServer server;

        public ImagesControllerTest(WebServerFixture fixture)
        {
            server = fixture.Server;
        }

        [Theory]
        [InlineData("api/images")]
        public async Task ShouldGetImages(string requestUri)
        {
            // Act
            string responseString = null;
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
            }
            var images = JsonConvert.DeserializeObject(responseString, typeof(IList<Model.Image>)) as IList<Model.Image>;

            // Assert
            Assert.Equal(14, images.Count);
        }
    }
}
