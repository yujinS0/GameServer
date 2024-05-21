using Microsoft.AspNetCore.Mvc;
using APIServer.DTO;
using APIServer.Repository;

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
        if (request == null || request.UserNum <= 0)
        {
            _logger.LogError("Invalid request data.");
            return BadRequest("Invalid request data.");
        }

        try
        {
            var userGameData = await _gameDb.GetUserGameDataAsync(request.UserNum);
            if (userGameData == null)
            {
                _logger.LogInformation("No user game data found for user ID {UserNum}.", request.UserNum);
                return NotFound($"No game data found for user ID {request.UserNum}.");
            }

            _logger.LogInformation("Retrieved game data for user ID {UserNum}.", request.UserNum);
            return Ok(new UserGameDataResponse
            {
                UserNum = userGameData.UserNum,
                UserId = userGameData.UserId,
                Level = userGameData.Level,
                Exp = userGameData.Exp,
                Win = userGameData.Win,
                Lose = userGameData.Lose,
                Draw = userGameData.Draw
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving game data for user ID {UserNum}.", request.UserNum);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
