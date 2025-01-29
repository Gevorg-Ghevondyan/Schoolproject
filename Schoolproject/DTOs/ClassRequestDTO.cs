using System.ComponentModel.DataAnnotations;

public class ClassRequestDTO
{
    [Required]
    public string Name { get; set; }

    public List<int>? SubjectIds { get; set; }
    public List<int>? TeacherIds { get; set; }
    public List<int>? StudentIds { get; set; }
}