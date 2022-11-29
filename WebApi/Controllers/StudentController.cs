using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly DataContext _context;

    public StudentController(ILogger<StudentController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<Student>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetStudent(int? id)
    {
        if (id == null){
            try
            {
                if (_context.Students == null || !_context.Students.Any()) return NotFound("No Students found in the database.");
                return Ok(_context.Students?.Take(5).ToList());
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        } 
        else 
        {
            try
            {
                var student = _context.Students?.Find(id);
                if (student == null) return NotFound($"Student with id {id} was not found.");
                return Ok(student);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
    
     [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult CreateStudent(Student studentToAdd){
        studentToAdd.Id = 0;
        try
        {
            _context.Students?.Add(studentToAdd);
            var result = _context.SaveChanges();

            if(result < 1) return Problem("Addition was not successful. Please try again");

            return Ok($"Successfully added student. Name: {studentToAdd.Name}, Birthdate: {studentToAdd.BirthDate}, College Program: {studentToAdd.CollegeProgram}, Year in College: {studentToAdd.YearInCollege}.");
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
    public IActionResult UpdateStudent(Student studentToEdit){
        if (studentToEdit.Id < 1) return BadRequest($"\"{studentToEdit.Id}\" is not a valid Id. Please use a valid Id.");

        try{
            var student = _context.Students?.Find(studentToEdit.Id);
            if (student == null) return NotFound($"Student with id \"{studentToEdit.Id}\" not found");
            
            if (studentToEdit.Name != null) student.Name = studentToEdit.Name;
            if (studentToEdit.BirthDate != DateTime.Parse("0001-01-01T00:00:00")) student.BirthDate = studentToEdit.BirthDate;
            if (studentToEdit.CollegeProgram != null) student.CollegeProgram = studentToEdit.CollegeProgram;
            if (studentToEdit.YearInCollege > 0) student.YearInCollege = studentToEdit.YearInCollege;

            _context.Students?.Update(student);
            var result = _context.SaveChanges();

            if (result < 1 ) return Problem("Update was not successful. Please try again");

            return Ok($"Update for student with id \"{studentToEdit.Id}\" was successful");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteStudent(int id)
    {
        try {
            var student = _context.Students?.Find(id);
            if (student == null) return NotFound($"Student with id \"{id}\" was not found");

            _context.Students?.Remove(student);
            var result = _context.SaveChanges();

            if (result < 1) return Problem("Delete was not successful. Please try again");

            return Ok($"Successfully deleted student with id \"{id}\", Student Name: {student.Name}");
        }
        catch(Exception e){
                return Problem(e.Message);
        }
    }

}
