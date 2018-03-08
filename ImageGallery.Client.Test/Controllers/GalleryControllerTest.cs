using ImageGallery.Client.Services;
using Moq;
using System;
using System.Net;
using System.Net.Http;
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
            var mockClient = new Mock<IImageGalleryHttpClient>();
            mockClient.Setup(m => m.GetClient()).Returns(() =>
            {
                // How to pass in a mocked HttpClient in a .NET test?
                // https://stackoverflow.com/questions/22223223/how-to-pass-in-a-mocked-httpclient-in-a-net-test
                var cli = new HttpClient(new FakeHttpMessageHandler(HttpStatusCode.BadRequest));
                cli.BaseAddress = new Uri("http://localhost/");
                return cli;
            });
            var controller = new GalleryController(mockClient.Object);

            // Act
            var task = controller.Index();

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await task);
            Output.WriteLine($"exception message: {exception.Message}");
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
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
            }

            return Task.FromResult(result);
        }
    }
}
