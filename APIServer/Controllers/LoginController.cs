using System.Text;
using System.Text.Json;
using APIServer.DTO;
using APIServer.Repository;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
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
                return new LoginResponse { Result = ErrorCode.ResponseFormatError };
            }

            if (validationResult.Result == ErrorCode.None)
            {
                var gameToken = await _gameRedis.SetUserTokenAsync(request.UserNum);
                var userData = await _gameDb.GetUserGameDataAsync(request.UserNum);

                if (userData == null)
                {
                    _logger.LogInformation("No user data found, creating new data for user ID {UserNum}", request.UserNum);
                    userData = await _gameDb.CreateUserGameDataAsync(request.UserNum, request.UserId);
                }

                _logger.LogInformation("Successfully authenticated user {UserNum} with token", request.UserNum);
                return new LoginResponse
                {
                    Result = ErrorCode.None,
                    Token = gameToken,
                    Uid = request.UserNum,
                    UserGameData = userData
                };
            }
            else
            {
                _logger.LogWarning("Token validation failed: {ErrorCode}", validationResult.Result);
                return new LoginResponse
                {
                    Result = validationResult.Result,
                    Token = "",
                    Uid = 0,
                    UserGameData = null
                };
            }
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "HTTP request to token validation service failed.");
            return new LoginResponse { Result = ErrorCode.ServerError };
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Error parsing JSON from token validation service.");
            return new LoginResponse { Result = ErrorCode.JsonParsingError };
        }
    }
}