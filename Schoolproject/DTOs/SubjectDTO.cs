using System.ComponentModel.DataAnnotations;

public class SubjectDTO
{
    [Required]
    [StringLength(20)]
    public string Name { get; set; }
    public List<int> TeacherIds { get; set; }
}
