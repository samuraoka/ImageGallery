using ImageGallery.Client.Controllers;
using ImageGallery.Client.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageGallery.Client
{
    public class GalleryController : Controller
    {
        private readonly IImageGalleryHttpClient imageGalleryHttpClient;

        public GalleryController(IImageGalleryHttpClient imageGalleryHttpClient)
        {
            this.imageGalleryHttpClient = imageGalleryHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            // call the API
            var httpClient = imageGalleryHttpClient.GetClient();

            var response = await httpClient.GetAsync("api/images");

            if (response.IsSuccessStatusCode)
            {
                var imagesAsString = await response.Content.ReadAsStringAsync();
                var images = JsonConvert.DeserializeObject<IList<Image>>(imagesAsString);
                var galleryIndexViewModel = new GalleryIndexViewModel(images);
                return View(galleryIndexViewModel);
            }

            throw new Exception($"A problem happend while calling the API: {response.ReasonPhrase}");
        }
    }
}
