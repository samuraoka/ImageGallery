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

        [Fact]
        public void ShouldSetGetTitleProperty()
        {
            // Arrange
            var title = "NewImageTitle";
            var obj = new ImageForCreation();

            // Act
            obj.Title = title;

            // Assert
            Assert.Equal(title, obj.Title);
        }
    }
}
