using StackExchange.Redis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiveServer.Model.DTO;
using Microsoft.Extensions.Logging;
using HiveServer.Services;
using HiveServer.Repository;

namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IHiveDb _hiveDb;
    private readonly IConnectionMultiplexer _redis;

    public LoginController(ILogger<LoginController> logger, IHiveDb hiveDb, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _hiveDb = hiveDb;
        _redis = redis;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var playerId = await _hiveDb.VerifyUser(request.Email, request.Password);
            if (playerId == 0)  // playerId가 0이면 로그인 실패
            {
                _logger.LogWarning("Login failed for Email: {Email}, invalid credentials", request.Email);
                return BadRequest(new LoginResponseDTO
                {
                    Success = false,
                    Message = "Invalid credentials",
                    userId = 0,
                    HiveToken = ""
                });
            }

            var token = Security.CreateAuthToken();
            var db = _redis.GetDatabase();
            await db.StringSetAsync($"user:{playerId}", token, TimeSpan.FromHours(1)); 

            return Ok(new LoginResponseDTO
            {
                Success = true,
                Message = "User logged in successfully.",
                userId = playerId,
                HiveToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login attempt failed for Email: {Email}", request.Email);
            return BadRequest(new LoginResponseDTO
            {
                Success = false,
                Message = "Login attempt failed due to an internal error",
                userId = 0,
                HiveToken = ""
            });
        }
    }
    
}
