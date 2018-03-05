using System.ComponentModel.DataAnnotations;
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

        [Fact]
        public void ShouldSetGetTitleProperty()
        {
            // Arrange
            var title = "TitleForUpdate";
            var obj = new ImageForUpdate();

            // Act
            obj.Title = title;

            // Assert
            Assert.Equal(title, obj.Title);
        }

        [Fact]
        public void ShouldGetRequiredAttributeFromTitleProperty()
        {
            var prop = typeof(ImageForUpdate).GetProperty(nameof(ImageForUpdate.Title));
            var attrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is RequiredAttribute);
        }
    }
}
