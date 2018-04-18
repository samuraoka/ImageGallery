using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Marvin.IDP
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Introduction to Razor Pages in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/?view=aspnetcore-2.0&tabs=visual-studio
            services.AddMvc();

            // IdentityServer4
            // https://www.nuget.org/packages/IdentityServer4/
            // Install-Package -Id IdentityServer4 -ProjectName Marvin.IDP
            //
            // Setup and Overview
            // http://docs.identityserver.io/en/release/quickstarts/0_overview.html#setup-and-overview
            //
            // Configure IdentityServer
            // http://docs.identityserver.io/en/release/quickstarts/1_client_credentials.html#configure-identityserver
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(Config.GetUsers())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Work with static files in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-2.0&tabs=aspnetcore2x
            app.UseStaticFiles();

            // Basic setup
            // http://docs.identityserver.io/en/release/quickstarts/0_overview.html#basic-setup
            app.UseIdentityServer();

            // Introduction to Razor Pages in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/?view=aspnetcore-2.0&tabs=visual-studio
            app.UseMvcWithDefaultRoute();
        }
    }
}
