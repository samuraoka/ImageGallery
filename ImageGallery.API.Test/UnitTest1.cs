using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ImageGallery.API.Test
{
    public class UnitTest1
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public UnitTest1()
        {
            // Microsoft.AspNetCore.TestHost
            // https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost/2.1.0-preview1-final
            // Install-Package -Id Microsoft.AspNetCore.TestHost -ProjectName ImageGallery.API.Test
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
            // Act
            var response = await client.GetAsync("api/values");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("[\"value1\",\"value2\"]", responseString);
        }
    }
}
