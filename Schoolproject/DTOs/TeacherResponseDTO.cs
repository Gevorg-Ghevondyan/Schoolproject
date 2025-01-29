namespace Schoolproject.DTOs
{
    public class TeacherResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<int> SubjectIds { get; set; }
        public List<int> ClassIds { get; set; }
    }
}