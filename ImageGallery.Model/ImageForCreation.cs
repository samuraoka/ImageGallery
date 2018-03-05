using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model.Test
{
    public class ImageForCreation
    {
        [Required]
        public string Title { get; set; }

        public byte[] Bytes { get; set; }
    }
}
