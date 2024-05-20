using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIServer.DTO;
using APIServer.Repository;
using Microsoft.Extensions.Logging;

namespace APIServer.Controllers;
[ApiController]
[Route("[controller]")]
public class GameDataController : ControllerBase
{
    private readonly IGameDb _gameDb;
    private readonly ILogger<GameDataController> _logger;

    public GameDataController(IGameDb gameDb, ILogger<GameDataController> logger)
    {
        _gameDb = gameDb;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<UserGameDataResponse>> GetGameData([FromBody] UserGameDataRequest request)
    {
        if (request == null || request.UserId <= 0)
        {
            _logger.LogError("Invalid request data.");
            return BadRequest("Invalid request data.");
        }

        try
        {
            var userGameData = await _gameDb.GetUserGameDataAsync(request.UserId);
            if (userGameData == null)
            {
                _logger.LogInformation("No user game data found for user ID {UserId}.", request.UserId);
                return NotFound($"No game data found for user ID {request.UserId}.");
            }

            _logger.LogInformation("Retrieved game data for user ID {UserId}.", request.UserId);
            return Ok(new UserGameDataResponse
            {
                UserId = userGameData.UserId,
                Email = userGameData.Email,
                Level = userGameData.Level,
                Exp = userGameData.Exp,
                Win = userGameData.Win,
                Lose = userGameData.Lose,
                Draw = userGameData.Draw
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving game data for user ID {UserId}.", request.UserId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
