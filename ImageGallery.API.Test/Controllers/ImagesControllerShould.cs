﻿using ImageGallery.Model;
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
    public class ImagesControllerShould : IDisposable
    {
        private TestServer server;
        private readonly ITestOutputHelper output;

        public ImagesControllerShould(ITestOutputHelper output)
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
        public async void ReturnListImages(string requestUri)
        {
            // Act
            int numberOfImages = -1;
            using (var client = server.CreateClient())
            {
                var res = await client.GetAsync(requestUri);
                res.EnsureSuccessStatusCode();
                var responseString = await res.Content.ReadAsStringAsync();
                var images = JsonConvert.DeserializeObject<IList<Image>>(responseString) as IList<Image>;
                numberOfImages = images.Count;
            }

            // Assert
            Assert.Equal(14, numberOfImages);
        }

        [Theory]
        [InlineData("api/images", "d70f656d-75a7-45fc-b385-e4daa834e6a8")]
        public async void ExposeGetImageMethod(string requestUri, string id)
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
        public async void ReturnImageForPassedId(string requestUri, string id)
        {
            // Act
            string responseString = null;
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(requestUri + "/" + id);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
            }
            var image = JsonConvert.DeserializeObject<Image>(responseString) as Image;

            // Assert
            Assert.IsType<Image>(image);
            Assert.Equal(id, image.Id.ToString());
        }

        [Theory]
        [InlineData("api/images", "../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg")]
        public async void ExposeCreateImageMethod(string requestUri, string imageFilePath)
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
        public async void ReturnBadRequestResponseIfRequestContentEmpty(string requestUri)
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
        public async void ReturnUnsupportedMediaTypeResponseIfMessageBodyNull(string requestUri)
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
        public async void ReturnUnprocessableEntityResponseIfImageTitleLengthOvers(string requestUri, int titleLength, string imageFilePath)
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
        public async void ReturnUnprocessableEntityResponseIfImageBytesNull(string requestUri, int titleLength, string imageFilePath)
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
        public async void SaveImageDataToDatabase(string requestUri, string imageFilePath)
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
        public async void SaveImageBytesToFileSystem(string requestUri, string imageFilePath)
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
        public async void ExposeDeleteImageMethod(string requestUri)
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
        public async void ReturnNotFoundResponseForNotExistId(string requestUri, string targetId)
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
        public async void DeleteImage(string requestUri, string imageFilePath)
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

        [Theory]
        [InlineData("api/images", "d70f656d-75a7-45fc-b385-e4daa834e6a8")]
        public async void ReturnUnsupportedMediaTypeResponseIfRequestBodyNull(string requestUri, string targetId)
        {
            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                HttpContent content = null;
                response = await client.PutAsync(string.Join("/", requestUri, targetId), content);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "d70f656d-75a7-45fc-b385-e4daa834e6a8", "")]
        public async void ReturnBadRequestResponseIfRequestBodyEmpty(string baseUri, string targetId, string content)
        {
            // Arrange
            var requestUri = string.Join('/', baseUri, targetId);
            var requestBody = new StringContent(content, Encoding.Unicode, JsonContentType);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PutAsync(requestUri, requestBody);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "ab46efdb-0384-400c-89cb-95bba1c500e9", null)]
        public async void ReturnUnprocessableEntityObjectResultIfTitleNull(string baseUri, string targetId, string title)
        {
            // Arrange
            var requestUri = string.Join('/', baseUri, targetId);
            var requestBody = CreateUpdateImageRequestBody(title);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PutAsync(requestUri, requestBody);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "ab46efdb-0384-400c-89cb-95bba1c500e9", "")]
        public async void ReturnUnprocessableEntityObjectResultIfTitleEmpty(string baseUri, string targetId, string title)
        {
            // Arrange
            var requestUri = string.Join('/', baseUri, targetId);
            var requestBody = CreateUpdateImageRequestBody(title);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PutAsync(requestUri, requestBody);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "ab46efdb-0384-400c-89cb-95bba1c500e9", 151)]
        public async void ReturnUnprocessableEntityObjectResultIfTitleLongerThan(string baseUri, string targetId, int titleLength)
        {
            // Arrange
            var requestUri = string.Join('/', baseUri, targetId);
            var requestBody = CreateUpdateImageRequestBody(string.Concat(Enumerable.Repeat('a', titleLength)));

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PutAsync(requestUri, requestBody);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "ab4999db-0384-400c-89cb-95bba1c500ff")]
        public async void ReturnNotFoundResponseIfInvalidIdProvided(string baseUri, string invalidId)
        {
            // Arrange
            var requestUri = string.Join('/', baseUri, invalidId);
            var requestBody = CreateUpdateImageRequestBody("NewTitle");

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PutAsync(requestUri, requestBody);
            }

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/images", "ab46efdb-0384-400c-89cb-95bba1c500e9", "NewImageTitle")]
        public async void UpdateImageTitle(string baseUri, string targetId, string newTitle)
        {
            // Arrange
            var originalImage = await GetImage(baseUri, targetId);
            var requestUri = string.Join('/', baseUri, targetId);
            var expectedTitle = $"{newTitle}-{Guid.NewGuid()}";
            var requestBody = CreateUpdateImageRequestBody(expectedTitle);

            // Act
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                response = await client.PutAsync(requestUri, requestBody);
                response.EnsureSuccessStatusCode();
            }
            var updatedImage = await GetImage(baseUri, targetId);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(targetId, updatedImage.Id.ToString());
            Assert.NotEqual(originalImage.Title, updatedImage.Title);
            Assert.Equal(expectedTitle, updatedImage.Title);
        }

        private async Task<Image> GetImage(string baseUri, string targetId)
        {
            HttpResponseMessage response = null;
            using (var client = server.CreateClient())
            {
                var requestUri = string.Join('/', baseUri, targetId);
                response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
            }
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Model.Image>(responseString) as Model.Image;
        }

        private StringContent CreateUpdateImageRequestBody(string title)
        {
            var data = new ImageForUpdate
            {
                Title = title
            };
            var serializedData = JsonConvert.SerializeObject(data);
            return new StringContent(serializedData, Encoding.Unicode, JsonContentType);
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
