using ImageGallery.Client.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.Client.Controllers.Test
{
    public class GalleryControllerTest
    {
        public ITestOutputHelper Output { get; }

        public GalleryControllerTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public async void ShouldGetExceptionWhenIndexActionFails()
        {
            // Arrange
            var mockClient = GetMockOfIImageGalleryHttpClient(HttpStatusCode.BadRequest);
            var controller = new GalleryController(mockClient.Object);

            // Act
            var task = controller.Index();

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await task);
            Output.WriteLine($"exception message: {exception.Message}");
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
        }

        [Fact]
        public async void ShouldGetGalleryIndexViewModelWhenIndexActionSuceeds()
        {
            // Arrange
            var mockClient = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(mockClient.Object);

            // Act
            // Testing controller logic in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GalleryIndexViewModel>(viewResult.Model);
            Assert.Equal(2, model.Images.Count());
        }

        private Mock<IImageGalleryHttpClient> GetMockOfIImageGalleryHttpClient(HttpStatusCode code)
        {
            var mockClient = new Mock<IImageGalleryHttpClient>();
            mockClient.Setup(m => m.GetClient()).Returns(() =>
            {
                // How to pass in a mocked HttpClient in a .NET test?
                // https://stackoverflow.com/questions/22223223/how-to-pass-in-a-mocked-httpclient-in-a-net-test
                var cli = new HttpClient(new FakeHttpMessageHandler(code));
                cli.BaseAddress = new Uri("http://localhost/");
                return cli;
            });
            return mockClient;
        }
    }

    // How to pass in a mocked HttpClient in a .NET test?
    // https://stackoverflow.com/questions/22223223/how-to-pass-in-a-mocked-httpclient-in-a-net-test
    internal class FakeHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpStatusCode statusCode;

        public FakeHttpMessageHandler(HttpStatusCode code)
        {
            statusCode = code;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = new HttpResponseMessage(statusCode);
            result.RequestMessage = request;

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    result.ReasonPhrase = "Because this client's handler always fails";
                    break;

                case HttpStatusCode.OK:
                    result.ReasonPhrase = "OK";
                    var images = new List<Image>
                    {
                        new Image()
                        {
                            Id = new Guid("9f35e705-637a-4bbe-8c35-402b2ecf7128"),
                            Title = "An image by Frank",
                            FileName = "4cdd494c-e6e1-4af1-9e54-24a8e80ea2b4.jpg",
                        },
                        new Image()
                        {
                            Id = new Guid("939df3fd-de57-4caf-96dc-c5e110322a96"),
                            Title = "An image by Frank",
                            FileName = "5c20ca95-bb00-4ef1-8b85-c4b11e66eb54.jpg",
                        },
                    };
                    string content = JsonConvert.SerializeObject(images);
                    result.Content = new StringContent(content, Encoding.Unicode, "application/json");
                    break;
            }

            return Task.FromResult(result);
        }
    }
}
