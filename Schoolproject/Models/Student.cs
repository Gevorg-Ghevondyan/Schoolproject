using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public int? ClassId { get; set; }

    [JsonIgnore]
    public Class Class { get; set; }
}