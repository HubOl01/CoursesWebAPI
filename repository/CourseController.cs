using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("courses")]
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CoursesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _db.Courses
            .Include(c => c.Students)
            .ToListAsync();

        var result = courses.Select(c => new CourseDto(
            c.Id,
            c.Name,
            c.Students.Select(s => new StudentDto(s.Id, s.FullName)).ToList()
        ));

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CourseDto dto)
    {
        var course = new Course { Id = Guid.NewGuid(), Name = dto.Name };
        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        return Ok(new { id = course.Id });
    }

    [HttpPost("{id:guid}/students")]
    public async Task<IActionResult> AddStudent(Guid id, [FromBody] StudentDto dto)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return NotFound();

        var student = new Student
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            CourseId = course.Id
        };
        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return Ok(new { id = student.Id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var course = await _db.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
        if (course == null) return NotFound();

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
