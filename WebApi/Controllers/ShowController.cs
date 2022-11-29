using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace contemp_programming_final_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowController : ControllerBase
    {
        private readonly ILogger<ShowController> _logger;
        private readonly DataContext _context;

        public ShowController(ILogger<ShowController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }
        // GET: api/<ShowController>
        [HttpGet]
        [ProducesResponseType(typeof(Show), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Show>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetShow(int? id)
    {
        if (id == null){
            try
            {
                if (_context.Shows == null || !_context.Shows.Any()) return NotFound("No Shows found in the database.");
                return Ok(_context.Shows?.Take(5).ToList());
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

        // POST api/<ShowController>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult CreateShow(Show showToAdd)
    {
        showToAdd.Id = 0;
        try
        {
            _context.Shows?.Add(showToAdd);
            var result = _context.SaveChanges();

            if(result < 1) return Problem("Addition was not successful. Please try again");

            return Ok("Addition successful");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

        // PUT api/<ShowController>/5
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateShow(Show showToEdit)
    {
        if (showToEdit.Id < 1) return BadRequest("Pleas provide a valid Id");

        try{
            var show = _context.Shows?.Find(showToEdit.Id);
            if (show == null) return NotFound("Show was not found");
            
            show.Name = showToEdit.Name;
            show.DateOfRelease = showToEdit.DateOfRelease;
            show.Seasons = showToEdit.Seasons;
            show.Genre = showToEdit.Genre;  

            _context.Shows?.Update(show);
            var result = _context.SaveChanges();

            if (result < 1 ) return Problem("Update was not successful. Please try again");

            return Ok("Update successful");
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }

        // DELETE api/<ShowController>/5
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
