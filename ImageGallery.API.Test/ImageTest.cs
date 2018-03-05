using ImageGallery.API.Entities;
using Xunit;

namespace ImageGallery.API.Test
{
    public class ImageTest
    {
        [Fact]
        public void ShouldCreateNewImageInstance()
        {
            // Act
            var obj = new Image();

            // Assert
            Assert.NotNull(obj);
            Assert.True(obj is Image);
        }
    }
}
