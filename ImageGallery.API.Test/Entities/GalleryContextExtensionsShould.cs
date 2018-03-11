using ImageGallery.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ImageGallery.API.Test.Entities
{
    public class GalleryContextExtensionsShould
    {
        private readonly DbContextOptions<GalleryContext> options;

        public GalleryContextExtensionsShould()
        {
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
        public void AddSeedDataToDatastore()
        {
            // Act
            using (var ctx = new GalleryContext(options))
            {
                ctx.EnsureSeedDataForContext();
            }

            // Assert
            List<Image> images = null;
            using (var ctx = new GalleryContext(options))
            {
                images = ctx.Images.ToList();
            }
            Assert.Equal(14, images.Count);
        }
    }
}
