using ImageGallery.Client.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

//TODO refactor this test class name and method name.
namespace ImageGallery.Client.Test.ViewModels
{
    public class EditImageViewModelTest
    {
        [Fact]
        public void ShouldBeAbleToCreateNewInstance()
        {
            // Act
            var newInstance = new EditImageViewModel
            {
                Title = "SomeTitle",
                Id = Guid.NewGuid(),
            };

            // Assert
            Assert.NotNull(newInstance);
            Assert.IsType<EditImageViewModel>(newInstance);
        }

        [Theory]
        [InlineData(nameof(EditImageViewModel.Title))]
        [InlineData(nameof(EditImageViewModel.Id))]
        public void ShouldGetRequiredAttributeFromProperty(string name)
        {
            // Act
            var propInfo = typeof(EditImageViewModel).GetProperty(name);
            var attrs = GetRequiredAttribute(propInfo);

            // Assert
            Assert.NotNull(attrs);
            Assert.IsType<RequiredAttribute[]>(attrs);
            Assert.Single(attrs);
            Assert.IsType<RequiredAttribute>(attrs[0]);
        }

        private static object[] GetRequiredAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false);
        }
    }
}
