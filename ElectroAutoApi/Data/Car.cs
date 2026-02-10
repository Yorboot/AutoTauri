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
    public string LicensePlate { get; set; } = null!;

    [Required]
    public string Brand { get; set; } = null!;

    [Required]
    public string Model { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Mileage { get; set; }

    public int? Seats { get; set; }

    public int? Doors { get; set; }

    public int? ProductionYear { get; set; }

    public int? Weight { get; set; }

    public string? Color { get; set; }

    public string? Image { get; set; }

    public DateTime? SoldAt { get; set; }

    public int Views { get; set; } = 0;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}