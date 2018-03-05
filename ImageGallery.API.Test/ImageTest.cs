using ImageGallery.API.Entities;
using System;
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

        [Fact]
        public void ShouldSetGetIdProperty()
        {
            // Arrange
            var newId = Guid.NewGuid();
            var obj = new Image();

            // Act
            obj.Id = newId;

            // Assert
            Assert.Equal(newId, obj.Id);
        }
    }
}
