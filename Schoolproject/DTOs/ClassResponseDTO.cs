namespace Schoolproject.DTOs
{
    public class ClassResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> SubjectIds { get; set; }
        public List<int> TeacherIds { get; set; }
        public List<int> StudentIds { get; set; }
    }

}