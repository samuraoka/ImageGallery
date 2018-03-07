using System;
using System.Net.Http;

namespace ImageGallery.Client
{
    public class GalleryController
    {
        public GalleryController()
        {
        }

        public void Index()
        {
            // TODO implemente index process

            var response = new HttpResponseMessage();

            // TODO implemente index process

            throw new Exception($"A problem happend while calling the API: {response.ReasonPhrase}");
        }
    }
}
