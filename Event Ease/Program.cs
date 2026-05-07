using Event_Ease.Data;
using Event_Ease.Services;
using Microsoft.EntityFrameworkCore;
using Event_Ease.Services;

namespace Event_Ease
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add services to the container.
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate(); // This does exactly what "Update-Database" does!
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
/* * AUTHOR: Emeris Eksteen
 * STUDENT NUMBER: st10437357
 * REPOSITORY: https://github.com/EMGPPT/cldv6211-g1-2026-part1-st10437357-Emeris
 * * CODE ATTRIBUTION:
 * Database configurations and MVC scaffolding were adapted from Microsoft Learn documentation.
 * Custom validation logic for double-booking prevention was developed using LINQ-to-Entities 
 * to satisfy the module requirements for business logic implementation.
 * * REFERENCE LIST:
 * [Paste the Harvard Reference list I gave you earlier here]
 * Amazon Web Services (AWS) (2026) Shared responsibility model. Available at: https://aws.amazon.com/compliance/shared-responsibility-model/ (Accessed: 15 April 2026).
 * Dell (2026) On-premise vs. cloud. Available at: https://www.dell.com/en-us/lp/dt/on-premise-vs-cloud (Accessed: 15 April 2026).
 * Future Processing (2024) Elasticity and scalability in cloud computing: what do you need to know?. Available at: https://www.future-processing.com/blog/elasticity-and-scalability-in-cloud-computing-what-do-you-need-to-know/ (Accessed: 15 April 2026).
 * Google Cloud (2026) What is cloud elasticity?. Available at: https://cloud.google.com/discover/what-is-cloud-elasticity (Accessed: 15 April 2026).
 * Hicron Software (2026) What is Platform as a Service (PaaS) and who can benefit?. Available at: https://hicronsoftware.com/blog/what-is-platform-as-a-service-and-who-can-benefit/ (Accessed: 15 April 2026).
 * MaibornWolff (2026) On premises vs cloud computing - what you need to know. Available at: https://www.maibornwolff.de/en/know-how/on-premises-vs-cloud-computing/ (Accessed: 15 April 2026).
 * Mell, P. and Grance, T. (2011) The NIST definition of cloud computing. Special Publication 800-145. Gaithersburg: National Institute of Standards and Technology.
 * Microsoft (2026) What is platform as a service (PaaS)?. Available at: https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/what-is-paas (Accessed: 15 April 2026).
 * Wiz (2026) The shared responsibility model explained w/examples. Available at: https://www.wiz.io/academy/cloud-security/shared-responsibility-model (Accessed: 15 April 2026).
 */