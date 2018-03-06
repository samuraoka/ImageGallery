using ImageGallery.API.Test.Fixture;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.API.Test.Controllers
{
    [Collection("Automapper collection")]
    public class ImagesControllerTest : IClassFixture<WebServerFixture>
    {
        private readonly TestServer server;
        private readonly ITestOutputHelper output;

        public ImagesControllerTest(WebServerFixture fixture, ITestOutputHelper output)
        {
            server = fixture.Server;
            this.output = output;
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
            var images = JsonConvert.DeserializeObject<IList<Model.Image>>(responseString) as IList<Model.Image>;

            // Assert
            Assert.Equal(14, images.Count);
        }

        [Theory]
        [InlineData("api/images", "d70f656d-75a7-45fc-b385-e4daa834e6a8")]
        public async Task ShouldAccessGetImageMethod(string requestUri, string id)
        {
            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.GetAsync(requestUri + "/" + id);
                response.EnsureSuccessStatusCode();
            }

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "d70f656d-75a7-45fc-b385-e4daa834e6a8")]
        public async Task ShouldGetImage(string requestUri, string id)
        {
            // Act
            string responseString = null;
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(requestUri + "/" + id);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
            }
            var image = JsonConvert.DeserializeObject<Model.Image>(responseString) as Model.Image;

            // Assert
            Assert.IsType<Model.Image>(image);
            Assert.Equal(id, image.Id.ToString());
        }
    }
}
