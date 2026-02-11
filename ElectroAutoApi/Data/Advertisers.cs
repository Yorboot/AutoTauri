using System.ComponentModel.DataAnnotations;

namespace ElectroAutoApi.Data;

public class Advertiser
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
}