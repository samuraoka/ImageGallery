using AutoMapper;
using ImageGallery.API.Helpers;
using ImageGallery.API.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageGallery.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Images")]
    public class ImagesController : Controller
    {
        private readonly IGalleryRepository galleryRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public ImagesController(IGalleryRepository galleryRepository, IHostingEnvironment hostingEnvironment)
        {
            this.galleryRepository = galleryRepository;
            this.hostingEnvironment = hostingEnvironment;
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

            // get this environment's web root path (the path
            // from which static content, like an image, is served)
            var webRootPath = hostingEnvironment.WebRootPath;

            // create the filename
            var filename = Guid.NewGuid().ToString() + ".jpg";

            // the full file path
            var filePath = Path.Combine($"{webRootPath}/images/{filename}");

            // write bytes and auto-close stream
            System.IO.File.WriteAllBytes(filePath, imageForCreation.Bytes);

            // fill out the filename
            imageEntity.FileName = filename;

            // add and save
            galleryRepository.AddImage(imageEntity);

            if (galleryRepository.Save() == false)
            {
                throw new Exception("Adding an image failed on save.");
            }

            var imageToReturn = Mapper.Map<Image>(imageEntity);

            // 201 Created
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201
            return CreatedAtRoute("GetImage", new { id = imageToReturn.Id }, imageToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteImage(Guid id)
        {
            var imageFromRepo = galleryRepository.GetImage(id);

            if (imageFromRepo == null)
            {
                return NotFound();
            }

            galleryRepository.DeleteImage(imageFromRepo);

            if (galleryRepository.Save() == false)
            {
                throw new Exception($"Deleting image with {id} failed on save.");
            }

            return NoContent();
        }
    }
}
