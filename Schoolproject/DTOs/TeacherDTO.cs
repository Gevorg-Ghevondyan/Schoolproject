using System.ComponentModel.DataAnnotations;

public class TeacherDTO
{
    [Required]
    [StringLength(20)]
    public string Name { get; set; }
    public List<int> SubjectIds { get; set; }
}