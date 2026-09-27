using PostService.Domain.Entities;

namespace PostService.Infrastructure.Data.Seeds
{
    public class LocationSeeds
    {
        public static async Task Seed(PostServiceDbContext context)
        {
            // Country: Türkiye
            if (!context.Countries.Any(c => c.Id == 1))
            {
                await context.Countries.AddAsync(new Country
                {
                    Id = 1,
                    Name = "Türkiye"
                });
                await context.SaveChangesAsync();
            }

            // City: Antalya
            if (!context.Cities.Any(c => c.Id == 1))
            {
                await context.Cities.AddAsync(new City
                {
                    Id = 1,
                    Name = "Antalya",
                    CountryId = 1
                });
                await context.SaveChangesAsync();
            }

            // Districts
            var districts = new List<District>
            {
                new District { Id = 1, Name = "Muratpaşa", CityId = 1 },
                new District { Id = 2, Name = "Kepez", CityId = 1 },
                new District { Id = 3, Name = "Alanya", CityId = 1 },
                new District { Id = 4, Name = "Manavgat", CityId = 1 },
                new District { Id = 5, Name = "Kemer", CityId = 1 },
                new District { Id = 6, Name = "Kaş", CityId = 1 },
                new District { Id = 7, Name = "Korkuteli", CityId = 1 },
                new District { Id = 8, Name = "Finike", CityId = 1 },
                new District { Id = 9, Name = "Gazipaşa", CityId = 1 },
                new District { Id = 10, Name = "Serik", CityId = 1 },
                new District { Id = 11, Name = "Kumluca", CityId = 1 },
                new District { Id = 12, Name = "Konyaaltı", CityId = 1 }
            };

            foreach (var district in districts)
            {
                if (!context.Districts.Any(d => d.Id == district.Id))
                {
                    await context.Districts.AddAsync(district);
                }
            }
            await context.SaveChangesAsync();
        }
    }  
}
