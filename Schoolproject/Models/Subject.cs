using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Subject
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    // Many-to-many relationship with Teachers
    public List<int> TeacherIds { get; set; }

    // Navigation property for the many-to-many relationship with Classes
    [JsonIgnore]
    public List<Class> Classes { get; set; }

    [JsonIgnore]
    public List<Teacher> Teachers { get; set; }
}