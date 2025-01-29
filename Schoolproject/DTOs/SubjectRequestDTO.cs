using System.ComponentModel.DataAnnotations;

public class SubjectRequestDTO
{
    [Required]
    public string Name { get; set; }

    public List<int>? TeacherIds { get; set; }
}