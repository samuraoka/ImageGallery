using Xunit;

namespace ImageGallery.Model.Test
{
    public class ImageForUpdateTest
    {
        [Fact]
        public void ShouldCreateNewInstance()
        {
            // Act
            var obj = new ImageForUpdate();

            // Assert
            Assert.NotNull(obj);
            Assert.True(obj is ImageForUpdate);
        }
    }
}
