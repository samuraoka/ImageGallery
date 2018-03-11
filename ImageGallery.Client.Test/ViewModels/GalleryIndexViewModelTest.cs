using ImageGallery.Client.ViewModels;
using ImageGallery.Model;
using System.Collections.Generic;
using Xunit;

//TODO refactor this test class name and method name.
namespace ImageGallery.Client.Test.ViewModels
{
    public class GalleryIndexViewModelTest
    {
        [Fact]
        public void ShouldBeAbleToCreateNewInstance()
        {
            // Act
            var images = new List<Image>();
            var instance = new GalleryIndexViewModel(images);

            // Assert
            Assert.NotNull(instance);
            Assert.IsType<GalleryIndexViewModel>(instance);
        }

        [Fact]
        public void ShouldGetImagesProperty()
        {
            // Act
            var images = new List<Image>();
            var instance = new GalleryIndexViewModel(images);

            // Assert
            Assert.Same(images, instance.Images);
        }
    }
}
