using ImageGallery.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.API.Test.Controllers
{
    [Collection("Automapper collection")]
    public class ImagesControllerTest : IDisposable
    {
        private TestServer server;
        private readonly ITestOutputHelper output;

        public ImagesControllerTest(ITestOutputHelper output)
        {
            this.output = output;

            // Microsoft.AspNetCore.TestHost
            // https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost/2.1.0-preview1-final
            // Install-Package -Id Microsoft.AspNetCore.TestHost -ProjectName ImageGallery.API.Test
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            // Set web root path for test environment
            var hostingEnvironment = server.Host.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            hostingEnvironment.WebRootPath = Environment.CurrentDirectory;

            // Create Directory
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "images"));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (server != null)
                {
                    server.Dispose();
                    server = null;
                }

                // Delete Directory
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "images"), recursive: true);
            }
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
        [InlineData("api/images", "../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async Task ShouldAccessCreateImageMethod(string requestUri, string imageFilePath)
        {
            // Arrange
            var title = "New Image " + Guid.NewGuid().ToString();
            var content = await CreateUploadImageContent(title, imageFilePath);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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
        [InlineData("api/images", 151, "../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async void ShouldGetUnprocessableEntityResponseIfImageTitleLengthOvers(string requestUri, int titleLength, string imageFilePath)
        {
            // Arrange
            // Best way to repeat a character in C#
            // https://stackoverflow.com/questions/411752/best-way-to-repeat-a-character-in-c-sharp
            var title = string.Concat(Enumerable.Repeat("a", titleLength));
            var content = await CreateUploadImageContent(title, imageFilePath);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, content);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            // HTTP response status codes
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
            // Getting Http Status code number (200, 301, 404, etc.) from HttpWebRequest and HttpWebResponse
            // https://stackoverflow.com/questions/1330856/getting-http-status-code-number-200-301-404-etc-from-httpwebrequest-and-ht
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", 150, null)]
        public async void ShouldGetUnprocessableEntityResponseIfImageBytesIsNull(string requestUri, int titleLength, string imageFilePath)
        {
            // Arrange
            // Best way to repeat a character in C#
            // https://stackoverflow.com/questions/411752/best-way-to-repeat-a-character-in-c-sharp
            var title = string.Concat(Enumerable.Repeat("a", titleLength));
            var content = await CreateUploadImageContent(title, imageFilePath);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, content);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            // HTTP response status codes
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
            // Getting Http Status code number (200, 301, 404, etc.) from HttpWebRequest and HttpWebResponse
            // https://stackoverflow.com/questions/1330856/getting-http-status-code-number-200-301-404-etc-from-httpwebrequest-and-ht
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async Task CouldUploadImageData(string requestUri, string imageFilePath)
        {
            // Arrange
            // Getting the number of images before uploading.
            int expectedNumberOfImages = await GetTheNumberOfImages(requestUri);
            expectedNumberOfImages += 1;
            // Creating an upload data
            var title = "New Image " + Guid.NewGuid().ToString();
            var content = await CreateUploadImageContent(title, imageFilePath);

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

        [Theory]
        [InlineData("api/images", "../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async void ShouldGetSavedImageFromFileSystem(string requestUri, string imageFilePath)
        {
            // Arrange
            // Creating an upload data
            var title = "New Image " + Guid.NewGuid().ToString();
            var content = await CreateUploadImageContent(title, imageFilePath);
            var expectedImage = await File.ReadAllBytesAsync(imageFilePath);

            // Act
            // Uploading...
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
            }
            var resBody = await response.Content.ReadAsStringAsync();
            var createdImage = JsonConvert.DeserializeObject<Model.Image>(resBody);
            var samvedImage = await File.ReadAllBytesAsync($"{Environment.CurrentDirectory}/images/{createdImage.FileName}");

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(expectedImage, samvedImage);
        }

        [Theory]
        [InlineData("api/images")]
        public async void ShouldBeAbleToAccessDeleteImageMethod(string requestUri)
        {
            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.DeleteAsync(requestUri);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "../../../../TestData/6b32c074-65af-4f2b-9f3a-1b2d4deb7050.jpg")]
        public async void ShouldGetNotFoundResponseIfPassesNotExistId(string requestUri, string targetId)
        {
            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.DeleteAsync(String.Join('/', requestUri, targetId));
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async void ShouldBeAbleToDeleteImage(string requestUri, string imageFilePath)
        {
            // Arrange
            var uploadedImage = await UploadImage(requestUri, imageFilePath);
            var expecteNumberOfImages = await GetTheNumberOfImages(requestUri);
            expecteNumberOfImages -= 1;

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.DeleteAsync(string.Join('/', requestUri, uploadedImage.Id));
                response.EnsureSuccessStatusCode();
            }

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(expecteNumberOfImages, await GetTheNumberOfImages(requestUri));
        }

        private async Task<Image> UploadImage(string requestUri, string imageFilePath)
        {
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                var title = "New Image " + Guid.NewGuid().ToString();
                var requestBody = await CreateUploadImageContent(title, imageFilePath);
                response = await client.PostAsync(requestUri, requestBody);
                response.EnsureSuccessStatusCode();
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Image>(responseBody);
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

        private async Task<StringContent> CreateUploadImageContent(string title, string imageFilePath)
        {
            output.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
            var uploadData = new ImageForCreation
            {
                Title = title,
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
