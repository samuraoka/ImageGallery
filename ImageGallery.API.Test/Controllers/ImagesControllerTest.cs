using ImageGallery.API.Test.Fixture;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            var images = JsonConvert.DeserializeObject<IList<Model.Image>>(responseString) as IList<Model.Image>;

            // Assert
            Assert.Equal(14, images.Count);
        }

        [Theory]
        [InlineData("api/images/GetImage", "d70f656d-75a7-45fc-b385-e4daa834e6a8")]
        public async Task ShouldAccessGetImageMethod(string requestUri, string id)
        {
            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                // Build query string for System.Net.HttpClient get
                // https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["id"] = id;
                response = await client.GetAsync(requestUri + "?" + query.ToString());
                response.EnsureSuccessStatusCode();
            }

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
