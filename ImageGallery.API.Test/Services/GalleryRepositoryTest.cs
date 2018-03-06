using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;

namespace ImageGallery.API.Test.Services
{
    public class GalleryRepositoryTest : IDisposable
    {
        private TestServer server;

        public GalleryRepositoryTest()
        {
            // Microsoft.AspNetCore.TestHost
            // https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost/2.1.0-preview1-final
            // Install-Package -Id Microsoft.AspNetCore.TestHost -ProjectName ImageGallery.API.Test
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (server != null)
                {
                    server.Dispose();
                    server = null;
                }
            }
        }

        [Fact]
        public void ShouldGetGalleryRepository()
        {
            // Act
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository));

            // Assert
            Assert.NotNull(repo);
            Assert.IsAssignableFrom<IGalleryRepository>(repo);
        }

        [Fact]
        public void CouldGetImages()
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;

            // Act
            var images = repo.GetImages();

            // Assert
            Assert.NotNull(images);
        }

        [Fact]
        public void CouldGetOrderedByTitleImages()
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;

            // Act
            var images = repo.GetImages();

            // Assert
            Image preceder = null;
            foreach (var image in images)
            {
                if (preceder != null)
                {
                    Assert.True(preceder.Title.CompareTo(preceder.Title) <= 0);
                    preceder = image;
                }
            }
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void ShouldGetNullIfNotExist(string notExistingId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(notExistingId);

            // Act
            var image = repo.GetImage(id);

            // Assert
            Assert.Null(image);
        }

        [Theory]
        [InlineData("25320c5e-f58a-4b1f-b63a-8ee07a840bdf")]
        public void CouldGetExistingFirstImage(string existingId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(existingId);

            // Act
            var image = repo.GetImage(id);

            // Assert
            Assert.NotNull(image);
            Assert.Equal(id, image.Id);
            Assert.Equal("An image by Frank", image.Title);
            Assert.Equal("3fbe2aea-2257-44f2-b3b1-3d8bacade89c.jpg", image.FileName);
            Assert.Equal("d860efca-22d9-47fd-8249-791ba61b07c7", image.OwnerId);
        }

        [Theory]
        [InlineData("1efe7a31-8dcc-4ff0-9b2d-5f148e2989cc")]
        public void CouldGetExistingSecondImage(string existingId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(existingId);

            // Act
            var image = repo.GetImage(id);

            // Assert
            Assert.NotNull(image);
            Assert.Equal(id, image.Id);
            Assert.Equal("An image by Frank", image.Title);
            Assert.Equal("43de8b65-8b19-4b87-ae3c-df97e18bd461.jpg", image.FileName);
            Assert.Equal("d860efca-22d9-47fd-8249-791ba61b07c7", image.OwnerId);
        }

        [Theory]
        [InlineData("ae03d971-40a6-4350-b8a9-7b12e1d93d71")]
        public void CouldGetExistingLastImage(string existingId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(existingId);

            // Act
            var image = repo.GetImage(id);

            // Assert
            Assert.NotNull(image);
            Assert.Equal(id, image.Id);
            Assert.Equal("An image by Claire", image.Title);
            Assert.Equal("fdfe7329-e05c-41fb-a7c7-4f3226d28c49.jpg", image.FileName);
            Assert.Equal("b7539694-97e7-4dfe-84da-b4256e1ff5c7", image.OwnerId);
        }

        [Theory]
        [InlineData("ae03d971-40a6-4350-b8a9-7b12e1d93d71")]
        public void ShouldGetTrueIfImageExist(string existingId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(existingId);

            // Act
            var isExist = repo.ImageExists(id);

            // Assert
            Assert.True(isExist);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void ShouldGetFalseIfImageNotExist(string notExistingId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(notExistingId);

            // Act
            var isExist = repo.ImageExists(id);

            // Assert
            Assert.False(isExist);
        }

        [Fact]
        public void CouldAddAndSaveImageAnd()
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var image = new Image
            {
                Id = Guid.NewGuid(),
                Title = "New Title",
                FileName = "NewFileName.jpg",
                OwnerId = "NewOwnerId",
            };

            // Act
            repo.AddImage(image);
            var saveResult = repo.Save();
            var addedImage = repo.GetImage(image.Id);

            // Arrange
            Assert.True(saveResult);
            Assert.Equal(image.Id, addedImage.Id);
            Assert.Equal(image.Title, addedImage.Title);
            Assert.Equal(image.FileName, addedImage.FileName);
            Assert.Equal(image.OwnerId, addedImage.OwnerId);
        }

        [Theory]
        [InlineData("ae03d971-40a6-4350-b8a9-7b12e1d93d71")]
        public void CouldNotUpdateIdAndSaveImage(string targetId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var image = repo.GetImage(new Guid(targetId));

            // Act
            image.Id = Guid.NewGuid();
            Assert.Throws<InvalidOperationException>(() => repo.Save());
        }

        [Theory]
        [InlineData("ae03d971-40a6-4350-b8a9-7b12e1d93d71")]
        public void CouldUpdateAndSaveImage(string targetId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(targetId);
            var image = repo.GetImage(id);

            // Act
            image.Title = "New Title";
            image.FileName = "NewImage.jpg";
            image.OwnerId = "NewOwnerId";
            repo.UpdateImage(image);
            var saveResult = repo.Save();
            var updatedImage = repo.GetImage(id);

            // Assert
            Assert.True(saveResult);
            Assert.Equal(image.Id, updatedImage.Id);
            Assert.Equal("New Title", updatedImage.Title);
            Assert.Equal("NewImage.jpg", updatedImage.FileName);
            Assert.Equal("NewOwnerId", updatedImage.OwnerId);
        }

        [Theory]
        [InlineData("25320c5e-f58a-4b1f-b63a-8ee07a840bdf")]
        public void CouldRemoveAndSaveImage(string targetId)
        {
            // Arrange
            var repo = server.Host.Services.GetService(typeof(IGalleryRepository)) as IGalleryRepository;
            var id = new Guid(targetId);
            var image = repo.GetImage(id);

            // Act
            repo.DeleteImage(image);
            var saveResult = repo.Save();

            // Assert
            Assert.True(saveResult);
            Assert.False(repo.ImageExists(id));
        }
    }
}
