public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<int>? TeacherIds { get; set; } = new List<int>();
    public List<int>? ClassIds { get; set; } = new List<int>();

    public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    public List<Class> Classes { get; set; } = new List<Class>();
}