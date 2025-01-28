using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Class
{
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Name { get; set; }

    public List<int>? StudentIds { get; set; } = new List<int>(); 
    public List<int>? TeacherIds { get; set; } = new List<int>(); 
    public List<int>? SubjectIds { get; set; } = new List<int>(); 

    [JsonIgnore]  
    public List<Student> Students { get; set; } = new List<Student>(); 
    [JsonIgnore] 
    public List<Teacher> Teachers { get; set; } = new List<Teacher>(); 
    [JsonIgnore]
    public List<Subject> Subjects { get; set; } = new List<Subject>();
}