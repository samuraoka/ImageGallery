using ImageGallery.Client.Controllers.Test;
using ImageGallery.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageGallery.Client.Test.Mocks
{
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
                    string content = GetContent(request);
                    result.Content = new StringContent(content, Encoding.Unicode, "application/json");
                    break;
            }

            return Task.FromResult(result);
        }

        private string GetContent(HttpRequestMessage request)
        {
            object content = null;
            if (request.RequestUri.ToString().Equals(GalleryControllerTest.httpClientBaseAddress + "api/images"))
            {
                content = new List<Image> {
                    new Image
                    {
                        Id = new Guid("9f35e705-637a-4bbe-8c35-402b2ecf7128"),
                        Title = "An image by Frank",
                        FileName = "4cdd494c-e6e1-4af1-9e54-24a8e80ea2b4.jpg",
                    },
                    new Image
                    {
                        Id = new Guid("939df3fd-de57-4caf-96dc-c5e110322a96"),
                        Title = "An image by Frank",
                        FileName = "5c20ca95-bb00-4ef1-8b85-c4b11e66eb54.jpg",
                    },
                };
            }
            else
            {
                var segs = request.RequestUri.Segments;
                content = new Image
                {
                    Id = new Guid(segs.Last()),
                    Title = "Dummy Title",
                    FileName = "DummyFileName.jpg",
                };
            }
            return JsonConvert.SerializeObject(content);
        }
    }
}
