using System.ComponentModel.DataAnnotations;

public class ClassDTO
{
    [Required]
    [StringLength(10)]
    public string Name { get; set; }

    public List<int>? StudentIds { get; set; }
    public List<int>? TeacherIds { get; set; }
    public List<int>? SubjectIds { get; set; }
}