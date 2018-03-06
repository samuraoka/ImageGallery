using ImageGallery.API.Entities;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageGallery.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            var connectionString = Configuration["connectionStrings:imageGalleryDBConnectionString"];
            services.AddGalleryContext(connectionString);

            // register the repository
            services.AddScoped<IGalleryRepository, GalleryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, GalleryContext galleryContext)
        {
            //TODO add console logging to the LoggerFactory

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // TODO add a exception handler for production environment.
            }

            // Serve files inside of web root
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?tabs=aspnetcore2x#serve-files-inside-of-web-root
            app.UseStaticFiles();

            // DB migrations are applied
            // TODO galleryContext.Database.Migrate();
            galleryContext.Database.EnsureCreated();

            // seed the DB with data
            galleryContext.EnsureSeedDataForContext();

            app.UseMvc();
        }
    }

    public static class IServiceCollectionExtension
    {
        public static void AddGalleryContext(this IServiceCollection services, string connectionString)
        {
            if (connectionString != null)
            {
                services.AddDbContext<GalleryContext>(builder => builder.UseSqlServer(connectionString));
            }
            else
            {
                var databaseName = Guid.NewGuid().ToString();
                services.AddDbContext<GalleryContext>(builder => builder.UseInMemoryDatabase(databaseName));
            }
        }
    }
}
