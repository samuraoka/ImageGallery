using ImageGallery.Model;
using System.Collections.Generic;

namespace ImageGallery.Client.Controllers
{
    public class GalleryIndexViewModel
    {
        public IEnumerable<Image> Images { get; }

        public GalleryIndexViewModel(IList<Image> images)
        {
            Images = images;
        }
    }
}
