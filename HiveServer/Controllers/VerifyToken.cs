using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HiveServer.Model.DTO;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VerifyTokenController : ControllerBase
{
    private readonly ILogger<VerifyTokenController> _logger;
    private readonly IConnectionMultiplexer _redis;

    public VerifyTokenController(ILogger<VerifyTokenController> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
    }

    [HttpPost]
    public async Task<IActionResult> Verify([FromBody] VerifyTokeRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var db = _redis.GetDatabase();
        var storedToken = await db.StringGetAsync($"user:{request.UserId}");
        
        if (storedToken.IsNullOrEmpty)
        {
            _logger.LogInformation("No token found for user ID: {UserId}", request.UserId);
            return Ok(new VerifyTokenResponseDTO
            {
                Success = false,
                Message = "Token not found."
            });
        }

        if (storedToken == request.HiveToken)
        {
            return Ok(new VerifyTokenResponseDTO
            {
                Success = true,
                Message = "Token is valid."
            });
        }
        else
        {
            _logger.LogWarning("Invalid token for user ID: {UserId}", request.UserId);
            return Ok(new VerifyTokenResponseDTO
            {
                Success = false,
                Message = "Invalid token."
            });
        }
    }
}