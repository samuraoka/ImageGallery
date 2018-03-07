using AutoMapper;
using ImageGallery.API.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ImageGallery.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Images")]
    public class ImagesController : Controller
    {
        private readonly IGalleryRepository galleryRepository;

        public ImagesController(IGalleryRepository galleryRepository)
        {
            this.galleryRepository = galleryRepository;
        }

        [HttpGet]
        public IActionResult GetImages()
        {
            // get from repo
            var imagesFromRepo = galleryRepository.GetImages();

            // map to model
            // AutoMapper
            // https://www.nuget.org/packages/AutoMapper
            // Install-Package -Id AutoMapper -Project ImageGallery.API
            var imagesToReturn = Mapper.Map<IEnumerable<Model.Image>>(imagesFromRepo);

            return Ok(imagesToReturn);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public IActionResult GetImage(Guid id)
        {
            var imageFromRepo = galleryRepository.GetImage(id);

            if (imageFromRepo == null)
            {
                return NotFound();
            }

            var imageToReturn = Mapper.Map<Model.Image>(imageFromRepo);

            return Ok(imageToReturn);
        }

        [HttpPost]
        public IActionResult CreateImage([FromBody] ImageForCreation imageForCreation)
        {
            return Ok();
        }
    }
}
