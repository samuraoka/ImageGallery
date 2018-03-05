using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model.Test
{
    public class ImageForUpdate
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
    }
}
