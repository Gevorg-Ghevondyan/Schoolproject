using System.ComponentModel.DataAnnotations;

public class TeacherRequestDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public List<int>? SubjectIds { get; set; }
    public List<int>? ClassIds { get; set; }
}