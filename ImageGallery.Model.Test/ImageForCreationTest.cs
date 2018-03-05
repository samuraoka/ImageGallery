using System.IO;
using Xunit;
using System.ComponentModel.DataAnnotations;

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

        [Fact]
        public void ShouldGetRequiredAttributeFromTitleProperty()
        {
            // Act
            // Reflection - get attribute name and value on property
            // https://stackoverflow.com/questions/6637679/reflection-get-attribute-name-and-value-on-property
            var prop = typeof(ImageForCreation).GetProperty(nameof(ImageForCreation.Title));
            var attrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is RequiredAttribute);
        }

        [Fact]
        public void ShouldGetMaxLengthAttributeFromTitleProperty()
        {
            // Act
            var prop = typeof(ImageForCreation).GetProperty(nameof(ImageForCreation.Title));
            var attrs = prop.GetCustomAttributes(typeof(MaxLengthAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is MaxLengthAttribute);
            Assert.Equal(150, (attrs[0] as MaxLengthAttribute).Length);
        }

        [Fact]
        public void ShouldSetGetBytesProperty()
        {
            // Arrange
            // Best way to read a large file into a byte array in C#?
            // https://stackoverflow.com/questions/2030847/best-way-to-read-a-large-file-into-a-byte-array-in-c
            var bytes = File.ReadAllBytes("../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg");
            var obj = new ImageForCreation();

            // Act
            obj.Bytes = bytes;

            // Assert
            Assert.Equal(bytes, obj.Bytes);
        }

        [Fact]
        public void ShouldGetRequiredAttributeFromBytesProperty()
        {
            var prop = typeof(ImageForCreation).GetProperty(nameof(ImageForCreation.Bytes));
            var attrs = prop.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.Single(attrs);
            Assert.True(attrs[0] is RequiredAttribute);
        }
    }
}
