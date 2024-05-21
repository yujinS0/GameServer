using System.Collections.Concurrent;
using System.Text.Json;
using System.Text;
using APIServer.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;
[ApiController]
[Route("[controller]")]
public class MatchController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static ConcurrentQueue<string> _reqQueue = new();
    private static ConcurrentDictionary<string, int> _completeDic = new();

    private readonly ILogger<MatchController> _logger;

    public MatchController(IHttpClientFactory httpClientFactory, ILogger<MatchController> logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger;
    }


    [HttpPost("request")]
    public async Task<IActionResult> Match([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/request : {request.UserId}");

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // POST 요청 보내기 (헤더 포함하지 않음)
        var response = await client.PostAsync("http://localhost:5167/match/request", content);
        var responseData = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Match request successful: {responseData}");
            return Ok(responseData);
        }
        else
        {
            _logger.LogError($"Match request failed: {responseData}");
            return StatusCode((int)response.StatusCode, responseData);
        }
    }

    [HttpPost("ismatched")]
    public async Task<IActionResult> IsMatched([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/ismatched : {request.UserId}");

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // POST 요청 보내기 (헤더 포함하지 않음)
        var response = await client.PostAsync("http://localhost:5167/match/ismatched", content);
        var responseData = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"IsMatched request successful: {responseData}");
            return Ok(responseData);
        }
        else
        {
            _logger.LogError($"IsMatched request failed: {responseData}");
            return StatusCode((int)response.StatusCode, responseData);
        }
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelMatch([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/cancel : {request.UserId}");

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // POST 요청 보내기 (헤더 포함하지 않음)
        var response = await client.PostAsync("http://localhost:5167/match/cancel", content);
        var responseData = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"CancelMatch request successful: {responseData}");
            return Ok(responseData);
        }
        else
        {
            _logger.LogError($"CancelMatch request failed: {responseData}");
            return StatusCode((int)response.StatusCode, responseData);
        }
    }
}