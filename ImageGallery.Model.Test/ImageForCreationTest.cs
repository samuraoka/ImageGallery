using Xunit;

namespace ImageGallery.Model.Test
{
    public class ImageForCreationTest
    {
        [Fact]
        public void ShouldCreateNewInstance()
        {
            // Act
            var obj = new ImageForCreation();

            // Assert
            Assert.NotNull(obj);
            Assert.True(obj is ImageForCreation);
        }
    }
}
