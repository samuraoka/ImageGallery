using ImageGallery.Client.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;

//TODO refactor this test class name and method name.
namespace ImageGallery.Client.Test
{
    public class StartupTest : IDisposable
    {
        public TestServer Server { get; private set; }

        public StartupTest()
        {
            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
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
                if (Server != null)
                {
                    Server.Dispose();
                    Server = null;
                }
            }
        }

        [Fact]
        public void ShouldGetHttpContextAccessorService()
        {
            // Act
            var service = Server.Host.Services.GetService(typeof(IHttpContextAccessor));

            // Assert
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IHttpContextAccessor>(service);
        }

        [Fact]
        public void ShouldGetHttpContextAccessorServiceAsSingleton()
        {
            // Act
            var service1 = Server.Host.Services.GetService(typeof(IHttpContextAccessor));
            var service2 = Server.Host.Services.GetService(typeof(IHttpContextAccessor));

            // Assert
            Assert.Same(service1, service2);
        }

        [Fact]
        public void ShouldGetImageGalleryHttpClientService()
        {
            // Act
            var service = Server.Host.Services.GetService(typeof(IImageGalleryHttpClient));

            // Assert
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IImageGalleryHttpClient>(service);
        }
    }
}
