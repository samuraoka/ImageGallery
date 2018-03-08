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
    internal class MockImageGalleryApiHandler : DelegatingHandler
    {
        private const string ErrorMessage = "Because this client's handler always fails";
        private const string ApplicationJson = "application/json";
        private const string PathWithoutAfterSlash = "/api/images";
        private const string PathWithoutBosthSlash = "api/images";
        private const string PathWithoutBeforeSlash = "api/images/";
        private readonly HttpStatusCode statusCode;

        public MockImageGalleryApiHandler(HttpStatusCode code)
        {
            statusCode = code;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

            if (request.RequestUri.AbsolutePath.StartsWith(PathWithoutAfterSlash) == false)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = "request URL's absolute path need to start with api/images";
                return Task.FromResult(response);
            }

            switch (request.Method.Method)
            {
                case "GET":
                    response = DoGetProcessing(request, response);
                    break;

                case "PUT":
                    response = DoPutProcessing(request, response);
                    break;

                case "DELETE":
                    response = DoDeleteProcessing(request, response);
                    break;

                default:
                    response.StatusCode = HttpStatusCode.NotFound;
                    break;
            }

            return Task.FromResult(response);
        }

        private HttpResponseMessage DoDeleteProcessing(HttpRequestMessage request, HttpResponseMessage response)
        {
            return DoPutProcessing(request, response);
        }

        private HttpResponseMessage DoPutProcessing(HttpRequestMessage request, HttpResponseMessage response)
        {
            try
            {
                var guid = Guid.Parse(request.RequestUri.Segments.Last());
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        response.StatusCode = HttpStatusCode.OK;
                        break;

                    default:
                        response.ReasonPhrase = ErrorMessage;
                        response.StatusCode = HttpStatusCode.BadRequest;
                        break;
                }
            }
            catch (Exception ex)
            {
                response.ReasonPhrase = $"Request URI need a valid GUID to be updated or deleted. {ex.Message}";
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        private HttpResponseMessage DoGetProcessing(HttpRequestMessage request, HttpResponseMessage response)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        string content = GetContent(request);
                        response.Content = new StringContent(content, Encoding.Unicode, ApplicationJson);
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Content = new StringContent("", Encoding.Unicode, ApplicationJson);
                        response.ReasonPhrase = ex.Message;
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                    break;

                default:
                    response.ReasonPhrase = ErrorMessage;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    break;
            }
            return response;
        }

        private string GetContent(HttpRequestMessage request)
        {
            object content = null;
            string absolutePath = request.RequestUri.AbsolutePath;
            if (absolutePath.EndsWith(PathWithoutBosthSlash) || absolutePath.EndsWith(PathWithoutBeforeSlash))
            {
                content = CreateListOfImages();
            }
            else
            {
                var segs = request.RequestUri.Segments;
                content = CreateList(segs.Last());
            }
            return JsonConvert.SerializeObject(content);
        }

        private object CreateList(string id)
        {
            var guid = Guid.Parse(id);
            return new Image
            {
                Id = guid,
                Title = "Dummy Title",
                FileName = "DummyFileName.jpg",
            };
        }

        private object CreateListOfImages()
        {
            return new List<Image> {
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
    }
}
