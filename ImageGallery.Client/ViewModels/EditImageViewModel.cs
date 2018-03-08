using System;
using System.Collections.Generic;

namespace ImageGallery.Client.Controllers
{
    public class EditImageViewModel
    {
        public IEnumerable<char> Title { get; set; }

        public Guid Id { get; set; }
    }
}
