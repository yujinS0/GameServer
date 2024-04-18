using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APIServer.DTO;
using APIServer.Repository;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IGameDb _gameDb;
    private readonly IMemoryDb _gameRedis;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IHttpClientFactory httpClientFactory, IGameDb gameDb, IMemoryDb gameRedis, ILogger<LoginController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _gameDb = gameDb;
        _gameRedis = gameRedis;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] TokenValidationRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        _logger.LogInformation("Sending token validation request to external API with content: {Content}", content);

        try
        {
            var response = await client.PostAsync("http://localhost:5092/VerifyToken", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received response with status {StatusCode}: {ResponseBody}", response.StatusCode, responseBody);

            var validationResult = JsonSerializer.Deserialize<TokenValidationResponse>(responseBody);

            if (validationResult == null)
            {
                _logger.LogWarning("Failed to deserialize the token validation response.");
                return BadRequest("Invalid response format.");
            }

            if (validationResult.Success)
            {
                var gameToken = await _gameRedis.SetUserTokenAsync(request.UserID);
                var userData = await _gameDb.GetUserGameDataAsync(request.UserID);

                if (userData == null)
                {
                    _logger.LogInformation("No user data found, creating new data for user ID {UserID}", request.UserID);
                    userData = await _gameDb.CreateUserGameDataAsync(request.UserID);
                }

                _logger.LogInformation("Successfully authenticated user {UserID} with token {Token}", request.UserID, gameToken);
                return Ok(new { Token = gameToken, UserData = userData });
            }
            else
            {
                _logger.LogWarning("Token validation failed: {Message}", validationResult.Message);
                return Unauthorized("토큰 유효하지 않음: " + validationResult.Message);
            }
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "HTTP request to token validation service failed.");
            return StatusCode(500, "서버 에러: " + e.Message);
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Error parsing JSON from token validation service.");
            return BadRequest("Invalid JSON format in response.");
        }
    }
}