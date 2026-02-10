using Microsoft.EntityFrameworkCore;

namespace ElectroAutoApi;

public class AppDbContext: DbContext
{
    public DbSet<Data.Car> Cars { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;port=3306;database=ElectroAuto;user=root;password=;",ServerVersion.Parse("8.0.30"));
    }
    
}