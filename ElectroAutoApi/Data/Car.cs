using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroAutoApi.Data;

public class Car
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 2)]
    [RegularExpression(@"^[A-Z]{2}-\d{3}-[A-Z]{2}$", ErrorMessage = "License plate must be in format XX-123-XX")]
    public string LicensePlate { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Brand { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Model { get; set; } = null!;

    [Required]
    [Range(0, 100000000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Range(0, 999999)]
    public int Mileage { get; set; }

    [Range(2, 8)]
    public int? Seats { get; set; }

    [Range(2, 6)]
    public int? Doors { get; set; }

    [Range(1900, 2100)]
    public int? ProductionYear { get; set; }

    [Range(500, 5000)]
    public int? Weight { get; set; }

    [StringLength(30)]
    public string? Color { get; set; }

    [Url]
    [StringLength(500)]
    public string? Image { get; set; }

    public DateTime? SoldAt { get; set; }

    [Range(0, int.MaxValue)]
    public int Views { get; set; } = 0;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}