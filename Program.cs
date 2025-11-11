using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Main entry point for the MVC Blog Post Application.
/// This application demonstrates a simple blog system with post listing and detail views.
/// </summary>
public class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates and configures the host builder.
    /// Configures services, middleware, and routing for the application.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Configured IHostBuilder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    // Add MVC services to the dependency injection container
                    services.AddControllersWithViews();
                });

                webBuilder.Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

                    // Configure the HTTP request pipeline
                    if (!env.IsDevelopment())
                    {
                        // Use exception handler for production
                        app.UseExceptionHandler("/Home/Error");
                        
                        // Enable HSTS (HTTP Strict Transport Security) for production
                        app.UseHsts();
                    }

                    // Serve static files (CSS, images, etc.)
                    app.UseStaticFiles();

                    // Enable routing
                    app.UseRouting();

                    // Enable authorization
                    app.UseAuthorization();

                    // Configure endpoint routing with default route pointing to BlogController
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Blog}/{action=Index}/{id?}");
                    });
                });
            });
}
