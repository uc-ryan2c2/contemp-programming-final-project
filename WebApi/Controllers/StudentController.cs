using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly StudentContext _context;

    public StudentController(ILogger<StudentController> logger, StudentContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("GetStudent")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetStudent()
    {
        try
        {
            if (_context.Students == null || !_context.Students.Any()) return NotFound("No Students found in the database");
            return Ok(_context.Students.ToList());
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("GetStudentByID")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetStudentBYId(int id)
    {
        var Student = _context.Students?.Find(id);
        if (Student == null){
            return NotFound("The requested resource was not found");
        }
        return Ok(Student);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteStudent(int id)
    {
        try {
            var Student = _context.Students?.Find(id);
            if (Student == null) return NotFound($"Student with id {id} was not found");

            _context.Students?.Remove(Student);
            var result = _context.SaveChanges();

            if (result < 1) return Problem("Delete was not successful. Please try again");

            return Ok("Deletion successful");
        }
        catch(Exception e){
                return Problem(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult CreateStudent(Student StudentToAdd){
        StudentToAdd.Id = 0;
        try
        {
            _context.Students?.Add(StudentToAdd);
            var result = _context.SaveChanges();

            if(result < 1) return Problem("Addition was not successful. Please try again");

            return Ok("Addition successful");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateStudent(Student StudentToEdit){
        if (StudentToEdit.Id < 1) return BadRequest("Pleas provide a valid Id");

        try{
            var Student = _context.Students?.Find(StudentToEdit.Id);
            if (Student == null) return NotFound("Student was not found");
            
            Student.Name = StudentToEdit.Name;
            Student.BirthDate = StudentToEdit.BirthDate;
            Student.CollegeProgram = StudentToEdit.CollegeProgram;
            Student.YearInCollege = StudentToEdit.YearInCollege;

            _context.Students?.Update(Student);
            var result = _context.SaveChanges();

            if (result < 1 ) return Problem("Update was not successful. Please try again");

            return Ok("Update successful");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}
