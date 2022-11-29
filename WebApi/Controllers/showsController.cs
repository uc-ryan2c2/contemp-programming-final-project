using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace contemp_programming_final_project.Controllers
{
    [ApiController]
    [Route("api/shows/[controller]")]
    public class showsController : ControllerBase
    {
        private readonly ILogger<showsController> _logger;
        private readonly DataContext _context;

        public showsController(ILogger<showsController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }
        // GET: api/<showsController>
        [HttpGet]
        [ProducesResponseType(typeof(Shows), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Shows>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetStudent(int? id)
    {
        if (id == null){
            try
            {
                if (_context.Shows == null || !_context.Shows.Any()) return NotFound("No Shows found in the database.");
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
                var show = _context.Shows?.Find(id);
                if (show == null) return NotFound($"Show with id {id} was not found.");
                return Ok(show);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }

        // POST api/<showsController>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult CreateStudent(Student showToAdd)
    {
        showToAdd.Id = 0;
        try
        {
            _context.Students?.Add(showToAdd);
            var result = _context.SaveChanges();

            if(result < 1) return Problem("Addition was not successful. Please try again");

            return Ok("Addition successful");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

        // PUT api/<showsController>/5
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateStudent(Student showToEdit)
    {
        if (showToEdit.Id < 1) return BadRequest("Pleas provide a valid Id");

        try{
            var Student = _context.Students?.Find(showToEdit.Id);
            if (Student == null) return NotFound("Show was not found");
            
            Student.Name = showToEdit.Name;
            Student.BirthDate = showToEdit.BirthDate;
            Student.CollegeProgram = showToEdit.CollegeProgram;
            Student.YearInCollege = showToEdit.YearInCollege;

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

        // DELETE api/<showsController>/5
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteShow(int id)
        {
            try {
                var Show = _context.Shows?.Find(id);
                if (Show == null) return NotFound($"Show with id {id} was not found");

                _context.Shows?.Remove(Show);
                var result = _context.SaveChanges();

                if (result < 1) return Problem("Delete was not successful. Please try again");

                return Ok("Deletion successful");
            }
            catch(Exception e){
                    return Problem(e.Message);
            }
        }
    }
}
