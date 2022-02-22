using PlatformService.Models;
using Microsoft.EntityFrameworkCore;
namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("---> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations:{ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data.");
                var list = new List<Platform>();
                list.Add(new Platform { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" });
                list.Add(new Platform { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" });
                list.Add(new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" });

                context.Platforms.AddRange(list);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("----> We already have data.");
            }
        }
    }
}