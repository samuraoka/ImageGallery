using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;

namespace ImageGallery.API.Test.Fixture
{
    public class WebServerFixture : IDisposable
    {
        public TestServer Server { get; private set; }

        public WebServerFixture()
        {
            // Microsoft.AspNetCore.TestHost
            // https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost/2.1.0-preview1-final
            // Install-Package -Id Microsoft.AspNetCore.TestHost -ProjectName ImageGallery.API.Test
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
    }

    [CollectionDefinition("WebServer collection")]
    public class WebServerCollection : ICollectionFixture<WebServerFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
