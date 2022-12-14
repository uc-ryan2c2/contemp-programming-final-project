using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.GameController;

[ApiController]
[Route("[controller]")]
public class VideoGameController : ControllerBase
{
    private readonly ILogger<VideoGameController> _logger;
    private readonly DataContext _context;

    public VideoGameController(ILogger<VideoGameController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(VideoGame), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<VideoGame>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public IActionResult GetVideoGame(int? id)
    {
         if (id == null){
            try
            {
                if (_context.VideoGames == null || !_context.VideoGames.Any()) return NotFound("No Video Games found in the database.");
                return Ok(_context.VideoGames?.Take(5).ToList());
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
                var game = _context.VideoGames?.Find(id);
                if (game == null) return NotFound($"Video Game with id {id} was not found.");
                return Ok(game);
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
    public IActionResult CreateVideoGame(VideoGame newGame)
    {
        newGame.Id = 0;
        try
        {
            _context.VideoGames?.Add(newGame);
            var result = _context.SaveChanges();

            if (result < 1) return Problem("New addition was unsuccessful. PLease Try Again");

                return Ok(newGame);
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
    public IActionResult UpdateVideoGame(VideoGame GameToEdit)
    {
        if (GameToEdit.Id < 1) return BadRequest("Please enter another id");
        try
        {
        var game = _context.VideoGames?.Find(GameToEdit.Id);
        if (game == null) return NotFound("The video game you are looking for was not found");

        game.Name = GameToEdit.Name;
        game.ReleaseDate = GameToEdit.ReleaseDate;
        game.Genre = GameToEdit.Genre;
        game.NumberOfPLayers = GameToEdit.NumberOfPLayers;

        _context.VideoGames?.Update(game);
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
    public IActionResult DeleteVideoGame(int id)
    {
        try
        {
        var game = _context.VideoGames?.Find(id);
        if (game == null)
        return NotFound(id);

        _context.VideoGames?.Remove(game);
        var result = _context.SaveChanges();

        if (result < 1) return NotFound(id);

        return Ok(game);
        }
        catch(Exception e)
        {
            return Problem(e.Message);
        }
    }
}