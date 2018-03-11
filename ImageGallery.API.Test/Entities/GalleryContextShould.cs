using ImageGallery.API.Entities;
using ImageGallery.API.Test.Fixture;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ImageGallery.API.Test.Entities
{
    // EF Core In-Memory Database Provider
    // https://docs.microsoft.com/en-us/ef/core/providers/in-memory/
    //
    // Microsoft.EntityFrameworkCore.InMemory 
    // https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/2.1.0-preview1-final
    // Install-Package -Id Microsoft.EntityFrameworkCore.InMemory -Project ImageGallery.API.Test
    [Collection("WebServer collection")]
    public class GalleryContextShould
    {
        private readonly TestServer server;
        private readonly DbContextOptions<GalleryContext> options;

        public GalleryContextShould(WebServerFixture fixture)
        {
            server = fixture.Server;

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
        public void BeGotFromInjectionService()
        {
            // Act
            var context = server.Host.Services.GetService(typeof(GalleryContext));

            // Assert
            Assert.NotNull(context);
            Assert.IsType<GalleryContext>(context);
        }

        [Fact]
        public void ReturnImageDbSet()
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
