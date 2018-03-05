using System.IO;
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
    }
}
