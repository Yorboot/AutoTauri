using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ElectroAutoApi.Data;
using ElectroAutoApi.Data.SeedingData;

namespace ElectroAutoApi.Seeders;

public class AdvertiserSeeder
{
    private readonly AppDbContext _db;
    private readonly Random _random = new();

    public AdvertiserSeeder(AppDbContext db)
    {
        _db = db;
    }

    public void Seed(int count = 50)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var jsonPath = Path.Combine(basePath, "Data", "SeedData", "advertisers.seed.json");

        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"Seed data file not found at: {jsonPath}");
        }

        var json = File.ReadAllText(jsonPath);
        var seedData = JsonSerializer.Deserialize<AdvertiserSeedData>(json,options: new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;

        var advertisers = new List<Advertiser>();

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"Picking firstName from {seedData.FirstNames?.Count ?? 0} items");
            var firstName = Pick(seedData.FirstNames);
    
            Console.WriteLine($"Picking lastName from {seedData.LastNames?.Count ?? 0} items");
            var lastName = Pick(seedData.LastNames);
    
            var name = $"{firstName} {lastName}";
    
            Console.WriteLine($"Picking emailDomain from {seedData.EmailDomains?.Count ?? 0} items");
            var email = GenerateEmail(firstName, lastName, seedData.EmailDomains);
    
            Console.WriteLine($"Picking password from {seedData.CommonPasswords?.Count ?? 0} items");
            var plainPassword = Pick(seedData.CommonPasswords);

            advertisers.Add(new Advertiser
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(plainPassword)
            });
        }

        _db.Advertisers.AddRange(advertisers);
        _db.SaveChanges();

        Console.WriteLine($"Seeded {count} advertisers with hashed passwords");
        Console.WriteLine("Sample login credentials:");
        Console.WriteLine($"  Email: {advertisers[0].Email}");
        Console.WriteLine($"  Password: {seedData.CommonPasswords[0]} (hashed in DB)");
    }

    private T Pick<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("Cannot pick from an empty or null list");
        }
        return list[_random.Next(list.Count)];
    }

    private string GenerateEmail(string firstName, string lastName, IList<string> domains)
    {
        var username = $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(1, 999)}";
        var domain = Pick(domains);
        return $"{username}@{domain}";
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}