using ImageGallery.API.Test.Fixture;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Xunit;

namespace ImageGallery.API.Test.Controllers
{
    public class UnitTest1 : IClassFixture<WebServerFixture>
    {
        private readonly TestServer server;

        public UnitTest1(WebServerFixture fixture)
        {
            server = fixture.Server;
        }

        [Fact]
        public async Task Test1()
        {
            // Act
            string responseString = null;
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync("api/values");
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
            }

            // Assert
            Assert.Equal("[\"value1\",\"value2\"]", responseString);
        }
    }
}
