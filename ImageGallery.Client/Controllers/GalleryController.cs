using ImageGallery.Client.Services;
using System;
using System.Threading.Tasks;

namespace ImageGallery.Client
{
    public class GalleryController
    {
        private readonly IImageGalleryHttpClient imageGalleryHttpClient;

        public GalleryController(IImageGalleryHttpClient imageGalleryHttpClient)
        {
            this.imageGalleryHttpClient = imageGalleryHttpClient;
        }

        public async Task Index()
        {
            // call the API
            var httpClient = imageGalleryHttpClient.GetClient();

            var response = await httpClient.GetAsync("api/images");

            if (response.IsSuccessStatusCode)
            {
                // TODO implemente index process
                return;
            }

            throw new Exception($"A problem happend while calling the API: {response.ReasonPhrase}");
        }
    }
}
