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

        // TODO

        public Image GetImage(Guid id)
        {
            return context.Images.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Image> GetImages()
        {
            return context.Images.OrderBy(i => i.Title).ToList();
        }

        // TODO

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
