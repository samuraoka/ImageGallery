using System;
using Xunit;

namespace ImageGallery.API.Test.Fixture
{
    public class AutoMapperFixture : IDisposable
    {
        public AutoMapperFixture()
        {
            Program.InitializeAutoMapper();
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
                AutoMapper.Mapper.Reset();
            }
        }
    }

    [CollectionDefinition("Automapper collection")]
    public class AutoMapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
