using ImageGallery.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ImageGallery.API.Test
{
    // EF Core In-Memory Database Provider
    // https://docs.microsoft.com/en-us/ef/core/providers/in-memory/
    //
    // Microsoft.EntityFrameworkCore.InMemory 
    // https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/2.1.0-preview1-final
    // Install-Package -Id Microsoft.EntityFrameworkCore.InMemory -Project ImageGallery.API.Test
    public class GalleryContextTest
    {
        private readonly DbContextOptions<GalleryContext> options;

        public GalleryContextTest()
        {
            // Testing with InMemory
            // https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
            options = new DbContextOptionsBuilder<GalleryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var ctx = new GalleryContext(options))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }

        [Fact]
        public void ShouldGetImageDbSet()
        {
            // Act
            List<Image> images = null;
            using (var ctx = new GalleryContext(options))
            {
                images = ctx.Images.ToList();
            }

            // Assert
            Assert.NotNull(images);
        }
    }
}
