using ImageGallery.Client.Services;
using ImageGallery.Client.Test.Mocks;
using ImageGallery.Client.ViewModels;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

//TODO refactor this test class name and method name.
namespace ImageGallery.Client.Controllers.Test
{
    public class GalleryControllerTest
    {
        public const string httpClientBaseAddress = "http://localhost/";

        public ITestOutputHelper Output { get; }

        public GalleryControllerTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public async void ShouldGetExceptionWhenIndexActionFails()
        {
            // Arrange
            var mockClient = GetMockOfIImageGalleryHttpClient(HttpStatusCode.BadRequest);
            var controller = new GalleryController(mockClient.Object);

            // Act
            var task = controller.Index();

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => task);
            Output.WriteLine($"exception message: {exception.Message}");
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
        }

        [Fact]
        public async void ShouldGetGalleryIndexViewModelWhenIndexActionSuceeds()
        {
            // Arrange
            var mockClient = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(mockClient.Object);

            // Act
            // Testing controller logic in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GalleryIndexViewModel>(viewResult.Model);
            Assert.Equal(2, model.Images.Count());
        }

        [Fact]
        public async void ShouldGetExceptionWhenEditImageGetActionFails()
        {
            // Arrange
            var mockClient = GetMockOfIImageGalleryHttpClient(HttpStatusCode.BadRequest);
            var controller = new GalleryController(mockClient.Object);

            // Act
            var result = controller.EditImage(Guid.NewGuid());

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => result);
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
        }

        [Fact]
        public async void ShouldGetEditImageViewModelWhenEditImageActionSuceeds()
        {
            // Arrange
            var mockClient = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(mockClient.Object);
            var expectedId = Guid.NewGuid();

            // Adt
            var result = await controller.EditImage(expectedId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EditImageViewModel>(viewResult.Model);
            Assert.Equal(expectedId, model.Id);
            Assert.Equal("Dummy Title", model.Title);
        }

        [Fact]
        public async void ShouldGetExceptionWhenEditImagePostActionFails()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.BadRequest);
            var controller = new GalleryController(client.Object);
            var editImageViewModel = new EditImageViewModel();

            // Action
            var result = controller.EditImage(editImageViewModel);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => result);
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
        }

        [Fact]
        public async void ShouldGetEmptyViewModelWhenModelStateInvalid()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(client.Object);
            // Testing controller logic in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing
            controller.ModelState.AddModelError("Title", "Required");
            var editImageViewModel = new EditImageViewModel();

            // Act
            var result = await controller.EditImage(editImageViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async void ShouldGetRedirectToActionResultWhenEditImagePostHttpMethodSucceeds()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(client.Object);
            var editImageViewModel = new EditImageViewModel
            {
                Id = Guid.NewGuid(),
                Title = "New Image Title",
            };

            // Act
            var response = await controller.EditImage(editImageViewModel);

            // Assert
            var result = Assert.IsType<RedirectToActionResult>(response);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async void ShouldGetExceptionWhenDeleteImageMethodFails()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.BadRequest);
            var controller = new GalleryController(client.Object);
            var id = Guid.NewGuid();

            // Act
            var result = controller.DeleteImage(id);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => result);
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
        }

        [Fact]
        public async void ShouldGetRedirectToActionResultWhenDeleteImageMethodSucceeds()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(client.Object);
            var id = Guid.NewGuid();

            // Act
            var response = await controller.DeleteImage(id);

            // Assert
            var result = Assert.IsType<RedirectToActionResult>(response);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void ShouldGetViewResultWhenAddImageGetMethodSucceeds()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(client.Object);

            // Act
            var result = controller.AddImage();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async void ShouldGetExceptionWhenAddImagePostMethodFailed()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.BadRequest);
            var controller = new GalleryController(client.Object);
            var addImageViewModel = new AddImageViewModel();
            addImageViewModel.Files.Add(await GetFormFileAsync());

            // Act
            var result = controller.AddImage(addImageViewModel);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => result);
            Assert.Equal("A problem happend while calling the API: Because this client's handler always fails", exception.Message);
        }

        [Fact]
        public async void ShouldGetViewResultWhenAddImagePostMethodInvalidatePostedData()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(client.Object);
            controller.ModelState.AddModelError("Title", "Title is required");
            var addImageViewModel = new AddImageViewModel();

            // Action
            var response = await controller.AddImage(addImageViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(response);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async void ShouldGetRedirectToActionResultWhenAddImagePostMethodSucceeds()
        {
            // Arrange
            var client = GetMockOfIImageGalleryHttpClient(HttpStatusCode.OK);
            var controller = new GalleryController(client.Object);
            var addImageViewModel = new AddImageViewModel
            {
                Title = "New Image Title",
            };
            addImageViewModel.Files.Add(await GetFormFileAsync());

            // Action
            var response = await controller.AddImage(addImageViewModel);

            // Assert
            var result = Assert.IsType<RedirectToActionResult>(response);
            Assert.Equal("Index", result.ActionName);
        }

        private static async Task<FormFile> GetFormFileAsync()
        {
            var buffer = await File.ReadAllBytesAsync("../../../../TestData/6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg");
            var baseStream = new MemoryStream(buffer);
            var file = new FormFile(baseStream, baseStreamOffset: 0, length: buffer.Length,
                name: "6b33c074-65cf-4f2b-913a-1b2d4deb7050", fileName: "6b33c074-65cf-4f2b-913a-1b2d4deb7050.jpg");
            return file;
        }

        private Mock<IImageGalleryHttpClient> GetMockOfIImageGalleryHttpClient(HttpStatusCode code)
        {
            var mockClient = new Mock<IImageGalleryHttpClient>();
            mockClient.Setup(m => m.GetClient()).Returns(() =>
            {
                // How to pass in a mocked HttpClient in a .NET test?
                // https://stackoverflow.com/questions/22223223/how-to-pass-in-a-mocked-httpclient-in-a-net-test
                var cli = new HttpClient(new MockImageGalleryApiHandler(code));
                cli.BaseAddress = new Uri(httpClientBaseAddress);
                return cli;
            });
            return mockClient;
        }
    }
}
