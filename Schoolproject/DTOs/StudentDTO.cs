using System.ComponentModel.DataAnnotations;

public class StudentDTO
{
    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    public int? ClassId { get; set; }
}