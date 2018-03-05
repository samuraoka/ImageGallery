using ImageGallery.API.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ImageGallery.API.Test.Entities
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

        [Fact]
        public void ShouldGetKeyAttributeFromIdProperty()
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.Id));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(KeyAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is KeyAttribute);
        }

        [Fact]
        public void ShouldSetGetTitleProperty()
        {
            // Arrange
            var newTitle = "Title value for Image";
            var obj = new Image();

            // Act
            obj.Title = newTitle;

            // Assert
            Assert.Equal(newTitle, obj.Title);
        }

        [Fact]
        public void ShouldGetRequiredAttributeFromTitleProperty()
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.Title));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is RequiredAttribute);
        }

        [Theory]
        [InlineData(150)]
        public void ShouldGetMaxLengthAttributeFromTitleProperty(int length)
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.Title));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(MaxLengthAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is MaxLengthAttribute);
            Assert.Equal(length, (attrs[0] as MaxLengthAttribute).Length);
        }

        [Fact]
        public void ShouldSetGetFileNameProperty()
        {
            // Arrange
            var newFileName = "foo.jpg";
            var obj = new Image();

            // Act
            obj.FileName = newFileName;

            // Assert
            Assert.Equal(newFileName, obj.FileName);
        }

        [Fact]
        public void ShouldGetRequiredAttributeFromFileNameProperty()
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.FileName));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is RequiredAttribute);
        }

        [Theory]
        [InlineData(200)]
        public void ShouldGetMaxLengthAttributeFromFileNameProperty(int length)
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.FileName));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(MaxLengthAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is MaxLengthAttribute);
            Assert.Equal(length, (attrs[0] as MaxLengthAttribute).Length);
        }

        [Fact]
        public void ShouldSetGetOwnerIdProperty()
        {
            // Arrange
            var newOwnerId = "999-888-777-666";
            var obj = new Image();

            // Act
            obj.OwnerId = newOwnerId;

            // Assert
            Assert.Equal(newOwnerId, obj.OwnerId);
        }

        [Fact]
        public void ShouldGetRequiredAttributeFromOwnerIdProperty()
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.OwnerId));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is RequiredAttribute);
        }

        [Theory]
        [InlineData(50)]
        public void ShouldGetMaxLengthAttributeFromOwnerIdProperty(int length)
        {
            // Arrange
            var prop = typeof(Image).GetProperty(nameof(Image.OwnerId));

            // Act
            var attrs = prop.GetCustomAttributes(typeof(MaxLengthAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is MaxLengthAttribute);
            Assert.Equal(length, (attrs[0] as MaxLengthAttribute).Length);
        }
    }
}
