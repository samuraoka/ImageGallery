using Microsoft.EntityFrameworkCore;

namespace ImageGallery.API.Entities
{
    // Add-Migration -Name InitialMigration -Context GalleryContext -Project ImageGallery.API -StartupProject ImageGallery.API
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}
