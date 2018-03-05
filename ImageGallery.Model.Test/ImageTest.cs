using Xunit;

namespace ImageGallery.Model.Test
{
    public class ImageTest
    {
        [Fact]
        public void ShouldCreateNewInstance()
        {
            // Act
            var obj = new Image();

            // Assert
            Assert.NotNull(obj);
            Assert.True(obj is Image);
        }
    }
}
