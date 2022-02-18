using PlatformService.Models;
namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        
        private static void SeedData(AppDbContext context)
        {
             if(!context.Platforms.Any())
            {
                Console.WriteLine("Seeding data.");
                var list = new List<Platform>();
                list.Add(new Platform { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free"} );
                list.Add(new Platform { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free"} );
                list.Add(new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"} );

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