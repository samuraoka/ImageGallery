using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model.Test
{
    public class ImageForCreation
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        public byte[] Bytes { get; set; }
    }
}
