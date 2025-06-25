public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public List<Student> Students { get; set; } = [];
}
public record CourseDto(Guid Id, string Name, List<StudentDto> Students);
