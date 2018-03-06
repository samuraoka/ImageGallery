using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.API
{
    internal class GalleryRepository : IGalleryRepository, IDisposable
    {
        private GalleryContext context;

        public GalleryRepository(GalleryContext galleryContext)
        {
            context = galleryContext;
        }

        public bool ImageExists(Guid id)
        {
            return context.Images.Any(i => i.Id == id);
        }

        public Image GetImage(Guid id)
        {
            return context.Images.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Image> GetImages()
        {
            return context.Images.OrderBy(i => i.Title).ToList();
        }

        public void AddImage(Image image)
        {
            context.Images.Add(image);
        }

        public void UpdateImage(Image image)
        {
            // no code in this implementation
        }

        public void DeleteImage(Image image)
        {
            context.Images.Remove(image);

            // Note: in a real-life scenario, the image itself should also
            // be removed from disk. We don't do this in this demo
            // scenario, as we refill the DB with image URIs (that require
            // the actual files as well) for demo purposes.
        }

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
    }
}
