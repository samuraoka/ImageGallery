using System;
using Xunit;
using Xunit.Abstractions;

namespace ImageGallery.Client.Test
{
    public class GalleryControllerTest
    {
        public ITestOutputHelper Output { get; }

        public GalleryControllerTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public void ShouldGetExceptionWhenIndexActionErrors()
        {
            // Arrange
            var controller = new GalleryController();

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => controller.Index());
            Output.WriteLine($"exception message: {exception.Message}");
            Assert.StartsWith("A problem happend while calling the API:", exception.Message);
        }
    }
}
