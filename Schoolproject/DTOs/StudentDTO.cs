public class StudentDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int? ClassId { get; set; }  // Make this nullable
}
