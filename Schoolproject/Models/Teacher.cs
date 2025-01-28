using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Teacher
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    // Many-to-many relationship with Subjects
    public List<int> SubjectIds { get; set; }

    // Navigation property for the many-to-many relationship with Classes
    [JsonIgnore]
    public List<Class> Classes { get; set; }

    [JsonIgnore]
    public List<Subject> Subjects { get; set; }
}