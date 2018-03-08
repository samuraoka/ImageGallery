using ImageGallery.Client.Controllers;
using ImageGallery.Client.Services;
using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> EditImage(Guid id)
        {
            // call the API
            var httpClient = imageGalleryHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/images/{id}");
            if (response.IsSuccessStatusCode)
            {
                var imageAsString = await response.Content.ReadAsStringAsync();
                var image = JsonConvert.DeserializeObject<Image>(imageAsString);
                var editImageViewModel = new EditImageViewModel
                {
                    Id = image.Id,
                    Title = image.Title,
                };
                return View(editImageViewModel);
            }
            throw new Exception($"A problem happend while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImage(EditImageViewModel editImageViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            //TODO implement create data process

            // call the API
            var httpClient = imageGalleryHttpClient.GetClient();
            var response = await httpClient.PutAsync("", new StringContent(""));

            if (response.IsSuccessStatusCode)
            {
                //TODO return action object
            }

            throw new Exception($"A problem happend while calling the API: {response.ReasonPhrase}");
        }

        //TODO implement other action methods.
    }
}
