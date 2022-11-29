using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.HobbyController;

[ApiController]
[Route("[controller]")]
public class HobbyController : ControllerBase
{
    private readonly ILogger<HobbyController> _logger;
    private readonly DataContext _context;

    public HobbyController(ILogger<HobbyController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Hobby), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<Hobby>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetHobby(int? id)
    {
         if (id == null){
            try
            {
                if (_context.Hobbys == null || !_context.Hobbys.Any()) return NotFound("No hobbys found in the database.");
                return Ok(_context.Hobbys?.Take(5).ToList());
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
                var hobby = _context.Hobbys?.Find(id);
                if (hobby == null) return NotFound($"hobby with id {id} was not found.");
                return Ok(hobby);
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
    public IActionResult CreateHobby(Hobby newhobby)
    {
        newhobby.Id = 0;
        try
        {
            _context.Hobbys?.Add(newhobby);
            var result = _context.SaveChanges();

            if (result < 1) return Problem("New addition was unsuccessful. PLease Try Again");

                return Ok(newhobby);
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
    public IActionResult UpdateHobby(Hobby hobbyToEdit)
    {
        if (hobbyToEdit.Id < 1) return BadRequest("Please enter another id");
        try
        {
        var hobby = _context.Hobbys?.Find(hobbyToEdit.Id);
        if (hobby == null) return NotFound("The hobby you are looking for was not found");

        hobby.Name = hobbyToEdit.Name;
        hobby.StartOfHobby = hobbyToEdit.StartOfHobby;
        hobby.Category = hobbyToEdit.Category;
        hobby.YearsOfHavingHobby = hobbyToEdit.YearsOfHavingHobby;

        _context.Hobbys.Update(hobby);
        var result = _context.SaveChanges();

        if (result < 1) return Problem("Update was not successful, please try again");
        return Ok("Update Successful");
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
    public IActionResult DeleteHobby(int id)
    {
        try
        {
        var hobby = _context.Hobbys?.Find(id);
        if (hobby == null)
        return NotFound(id);

        _context.Hobbys?.Remove(hobby);
        var result = _context.SaveChanges();

        if (result < 1) return NotFound(id);

        return Ok(hobby);
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}