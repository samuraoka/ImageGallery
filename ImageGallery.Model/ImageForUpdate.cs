using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model.Test
{
    public class ImageForUpdate
    {
        [Required]
        public string Title { get; set; }
    }
}
