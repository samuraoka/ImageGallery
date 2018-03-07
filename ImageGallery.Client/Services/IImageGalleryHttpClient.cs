using System.Net.Http;

namespace ImageGallery.Client.Services
{
    public interface IImageGalleryHttpClient
    {
        HttpClient GetClient();
    }
}
