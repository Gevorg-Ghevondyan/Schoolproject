namespace Schoolproject.DTOs
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? ClassId { get; set; }
    }
}