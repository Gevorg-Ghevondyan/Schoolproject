public class Class
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Subject> Subjects { get; set; } = new List<Subject>();
    public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    public List<Student> Students { get; set; } = new List<Student>();

    public List<int> TeacherIds { get; set; }
    public List<int> StudentIds { get; set; }
    public List<int> SubjectIds { get; set; }
}