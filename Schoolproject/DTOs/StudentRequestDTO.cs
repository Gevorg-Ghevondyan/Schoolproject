using System.ComponentModel.DataAnnotations;

public class StudentRequestDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public int? ClassId { get; set; }
}