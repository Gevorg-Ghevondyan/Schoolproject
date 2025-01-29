public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public List<int>? SubjectIds { get; set; } = new List<int>();
    public List<int>? ClassIds { get; set; } = new List<int>();

    public List<Subject> Subjects { get; set; } = new List<Subject>();
    public List<Class> Classes { get; set; } = new List<Class>();
}