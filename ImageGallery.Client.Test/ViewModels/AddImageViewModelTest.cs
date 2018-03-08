using ImageGallery.Client.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Xunit;

namespace ImageGallery.Client.Test.ViewModels
{
    public class AddImageViewModelTest
    {
        [Fact]
        public void ShouldCreateNewInstance()
        {
            // Act
            var instance = new AddImageViewModel();

            // Assert
            Assert.NotNull(instance);
            Assert.IsType<AddImageViewModel>(instance);
        }
    }
}
