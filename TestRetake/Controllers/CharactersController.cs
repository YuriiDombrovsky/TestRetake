using Microsoft.AspNetCore.Mvc;
using TestRetake.DTOs;
using TestRetake.Exceptions;
using TestRetake.Services;

namespace TestRetake.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly IDbService _dbService;

    public CharactersController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{characterId}")]
    public async Task<IActionResult> GetCharacter(int characterId)
    {
        try
        {
            var character = await _dbService.GetCharacterById(characterId);
            return Ok(character);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{characterId}/backpacks")]
    public async Task<IActionResult> AddItems(int characterId, List<int> itemIds)
    {
        try
        {
            await _dbService.AddItemsToBackpack(characterId, itemIds);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }
}

