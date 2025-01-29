public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public int? ClassId { get; set; }

    public List<Class> Classes { get; set; } = new List<Class>();
}