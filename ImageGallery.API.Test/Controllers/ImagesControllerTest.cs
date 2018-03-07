using ImageGallery.API.Test.Fixture;
using ImageGallery.Model;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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
            int numberOfImages = await GetTheNumberOfImages(requestUri);

            // Assert
            Assert.Equal(14, numberOfImages);
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

        [Theory]
        [InlineData("api/images", null)]
        public async Task ShouldAccessCreateImageMethod(string requestUri, string imageFilePath)
        {
            // Arrange
            var content = await CreateUploadImageContent(imageFilePath);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images")]
        public async Task ShouldGetBadRequestResponseIfContentIsEmpty(string requestUri)
        {
            // Arrange
            // Parameter Binding in ASP.NET Web API
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
            var emptyContent = new StringContent("", Encoding.Unicode, JsonContentType);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, emptyContent);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images")]
        public async Task ShouldGetUnsupportedMediaTypeResponseIfMessageBodyNull(string requestUri)
        {
            // Arrange
            HttpContent emptyContnet = null;

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, emptyContnet);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async Task CouldUploadImageData(string requestUri, string imageFilePath)
        {
            // Arrange
            // Getting the number of images before uploading.
            int expectedNumberOfImages = await GetTheNumberOfImages(requestUri);
            expectedNumberOfImages += 1;
            // Creating an upload data
            var content = await CreateUploadImageContent(imageFilePath);

            // Act
            // Uploading...
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            // Getting the count of images, second time...
            int actualNumberofImages = await GetTheNumberOfImages(requestUri);

            // Assert
            Assert.Equal(expectedNumberOfImages, actualNumberofImages);
        }

        private async Task<int> GetTheNumberOfImages(string requestUri)
        {
            int numberOfImages = -1;
            using (var client = server.CreateClient())
            {
                var res = await client.GetAsync(requestUri);
                res.EnsureSuccessStatusCode();
                var responseString = await res.Content.ReadAsStringAsync();
                var images = JsonConvert.DeserializeObject<IList<Model.Image>>(responseString) as IList<Model.Image>;
                numberOfImages = images.Count;
            }
            return numberOfImages;
        }

        private async Task<StringContent> CreateUploadImageContent(string imageFilePath)
        {
            output.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
            var uploadData = new ImageForCreation
            {
                Title = "New Image " + Guid.NewGuid().ToString(),
                Bytes = null,
            };
            if (imageFilePath != null)
            {
                uploadData.Bytes = await File.ReadAllBytesAsync(imageFilePath);
            }
            var serializedImageForCreation = JsonConvert.SerializeObject(uploadData);
            return new StringContent(serializedImageForCreation, Encoding.Unicode, JsonContentType);
        }

        public const string JsonContentType = "application/json";
    }
}
