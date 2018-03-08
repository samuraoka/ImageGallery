using ImageGallery.Client.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Fact]
        public void ShouldBeAbleToSetGetFiles()
        {
            // Arrange
            var instance = new AddImageViewModel();
            var files = new List<IFormFile>();

            // Act
            instance.Files = files;

            // Assert
            Assert.Same(files, instance.Files);
        }

        [Fact]
        public void ShouldGetFilesPropertyAfterCreation()
        {
            // Arrange
            var instance = new AddImageViewModel();

            // Act
            var files = instance.Files;

            // Assert
            Assert.NotNull(files);
            Assert.IsType<List<IFormFile>>(files);
        }

        [Fact]
        public void ShouldSetGetTitleProperty()
        {
            // Arrange
            var instance = new AddImageViewModel();

            // Act
            instance.Title = "NewImageTitle";
            var t = instance.Title;

            // Assert
            Assert.IsType<string>(instance.Title);
            Assert.Same(t, instance.Title);
            Assert.Equal("NewImageTitle", t);
        }

        [Fact]
        public void ShouldGetRequiredAttributeFromTitleProperty()
        {
            // Arrange
            var propertyInfo = typeof(AddImageViewModel).GetProperty(nameof(AddImageViewModel.Title));

            // Act
            var attrs = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false);

            // Assert
            Assert.NotNull(attrs);
            Assert.IsType<RequiredAttribute[]>(attrs);
            Assert.Single(attrs);
            Assert.IsType<RequiredAttribute>(attrs[0]);
        }
    }
}
