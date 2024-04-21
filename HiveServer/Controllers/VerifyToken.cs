using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HiveServer.Model.DTO;
using StackExchange.Redis;
using System.Threading.Tasks;
using HiveServer.Services;
using HiveServer.Repository;

namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VerifyTokenController : ControllerBase
{
    private readonly ILogger<VerifyTokenController> _logger;
    private readonly IHiveRedis _hiveRedis;

    public VerifyTokenController(ILogger<VerifyTokenController> logger, IHiveRedis hiveRedis)
    {
        _logger = logger;
        _hiveRedis = hiveRedis;
    }

    [HttpPost]
public async Task<VerifyTokenResponse> Verify([FromBody] VerifyTokenRequest request)
{
    bool isValid = await _hiveRedis.ValidateTokenAsync(request.UserId, request.HiveToken);

    if (!isValid)
    {
        return new VerifyTokenResponse
        {
            Result = ErrorCode.VerifyTokenFail,
        };
    }

    return new VerifyTokenResponse
    {
        Result = ErrorCode.None,
    };
}
}
