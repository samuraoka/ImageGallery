﻿using System;
using Xunit;

//TODO refactor this test class name and method name.
namespace ImageGallery.Model.Test
{
    public class ImageTest
    {
        [Fact]
        public void ShouldCreateNewInstance()
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
            var id = Guid.NewGuid();
            var obj = new Image();

            // Act
            obj.Id = id;

            // Assert
            Assert.Equal(id, obj.Id);
        }

        [Fact]
        public void ShouldSetGetTitleProperty()
        {
            // Arrange
            var title = "New Image Title";
            var obj = new Image();

            // Act
            obj.Title = title;

            // Assert
            Assert.Equal(title, obj.Title);
        }

        [Fact]
        public void ShouldSetGetFileNameProperty()
        {
            // Arrange
            var fileName = "NewImageFileName.jpg";
            var obj = new Image();

            // Act
            obj.FileName = fileName;

            // Assert
            Assert.Equal(fileName, obj.FileName);
            
        }
    }
}
