public class Student
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
public record StudentDto(Guid Id, string FullName);
