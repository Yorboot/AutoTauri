using System.Text.Json;
using ElectroAutoApi.Data;
using ElectroAutoApi.Data.SeedingData;

namespace ElectroAutoApi.Seeders;

public class CarSeeder
{
    private readonly AppDbContext _db;
    private readonly Random _random = new();

    public CarSeeder(AppDbContext db)
    {
        _db = db;
    }

    public void Seed(int count = 250)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var jsonPath = Path.Combine(basePath, "Data", "SeedData", "cars.seed.json");
    
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"Seed data file not found at: {jsonPath}");
        }
    
        var json = File.ReadAllText(jsonPath);
        var seedData = JsonSerializer.Deserialize<CarSeedData>(json)!;

        var cars = new List<Car>();

        for (int i = 0; i < count; i++)
        {
            var brand = Pick(seedData.Brands);
            var model = Pick(seedData.Models[brand]);

            cars.Add(new Car
            {
                UserId = _random.Next(1, 51),
                LicensePlate = GenerateLicensePlate(),
                Brand = brand,
                Model = model,
                Price = _random.Next(seedData.PriceRange.Min, seedData.PriceRange.Max),
                Mileage = _random.Next(seedData.MileageRange.Min, seedData.MileageRange.Max),
                Seats = _random.Next(seedData.SeatsRange.Min, seedData.SeatsRange.Max),
                Doors = _random.Next(seedData.DoorsRange.Min, seedData.DoorsRange.Max),
                ProductionYear = _random.Next(seedData.ProductionYearRange.Min, seedData.ProductionYearRange.Max),
                Weight = _random.Next(seedData.WeightRange.Min, seedData.WeightRange.Max),
                Color = Pick(seedData.Colors),
                Image = $"https://example.com/images/car_{i + 1}.jpg",
                SoldAt = _random.Next(0, 10) > 7 ? DateTime.Now.AddDays(-_random.Next(1, 365)) : null,
                Views = _random.Next(seedData.ViewsRange.Min, seedData.ViewsRange.Max),
                CreatedAt = DateTime.Now.AddDays(-_random.Next(1, 730)),
                UpdatedAt = DateTime.Now.AddDays(-_random.Next(0, 30))
            });
        }

        _db.Cars.AddRange(cars);
        _db.SaveChanges();
    }

    private T Pick<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("Cannot pick from an empty or null list");
        }
        return list[_random.Next(list.Count)];
    }

    private string GenerateLicensePlate()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";

        return $"{letters[_random.Next(26)]}{letters[_random.Next(26)]}-" +
               $"{numbers[_random.Next(10)]}{numbers[_random.Next(10)]}{numbers[_random.Next(10)]}-" +
               $"{letters[_random.Next(26)]}{letters[_random.Next(26)]}";
    }
}