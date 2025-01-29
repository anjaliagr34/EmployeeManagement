using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore; // Add this using statement
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;  // Inject IConfiguration to access appsettings.json or environment variables
        }
        // Add by Anjali Agrawal
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding MVC services to the application
            services.AddDbContext<EmployeeManagementDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("EmployeeManagementDB")));
            services.AddMvc()
                .AddXmlDataContractSerializerFormatters();  // This allows the application to return XML responses

            // Optionally, add any other services needed for the app
            // Example: Adding Entity Framework Core or any other custom services
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlServer(_config.GetConnectionString("EmployeeManagementDB")));
        }

        // This method is called by the runtime to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Enable the Developer Exception Page only in the Development environment
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");  // Custom error page for production
                app.UseHsts();  // Adds HTTP Strict Transport Security (HSTS) header
            }

            // Enables HTTPS redirection
            app.UseHttpsRedirection();

            // Serves static files from the wwwroot folder (e.g., images, CSS, JS files)
            app.UseStaticFiles();

            // Configures the routing middleware
            app.UseRouting();

            // Configures endpoints for the application
            app.UseEndpoints(endpoints =>
            {
                // Default route
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Log that the app has started
            logger.LogInformation("Application started successfully!");
        }
    }
}
