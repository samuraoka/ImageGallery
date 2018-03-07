using AutoMapper;
using ImageGallery.API.Helpers;
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
            if (imageForCreation == null)
            {
                return BadRequest();
            }

            // Introduction to model validation in ASP.NET Core MVC
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation
            if (ModelState.IsValid == false)
            {
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);
            }

            // Automapper maps only the Title in our configuration
            var imageEntity = Mapper.Map<Entities.Image>(imageForCreation);

            // Create an iamge from the passed-in bytes (Base64), and
            // set the filename on the image

            // TODO implement how to save image to the file system.

            // add and save
            galleryRepository.AddImage(imageEntity);

            if (galleryRepository.Save() == false)
            {
                throw new Exception("Adding an image failed on save.");
            }

            // TODO implement create image process.

            return Ok();
        }
    }
}
