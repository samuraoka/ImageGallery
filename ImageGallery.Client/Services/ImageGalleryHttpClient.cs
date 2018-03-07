using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ImageGallery.Client.Services
{
    public class ImageGalleryHttpClient : IImageGalleryHttpClient
    {
        private HttpClient httpClient = new HttpClient();

        public ImageGalleryHttpClient()
        {
            // TODO implement other code here
        }

        public HttpClient GetClient()
        {
            httpClient.BaseAddress = new Uri("http://localhost:58828/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
