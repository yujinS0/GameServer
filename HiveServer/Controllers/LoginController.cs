using Microsoft.AspNetCore.Mvc;
using HiveServer.Model.DTO;
using HiveServer.Services;
using HiveServer.Repository;

namespace HiveServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IHiveDb _hiveDb;
    private readonly IHiveRedis _hiveRedis;


    public LoginController(ILogger<LoginController> logger, IHiveDb hiveDb, IHiveRedis hiveRedis, IConfiguration config)
    {
        _logger = logger;
        _hiveDb = hiveDb;
        _hiveRedis = hiveRedis;
    }

    [HttpPost]
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var (error, userNum) = await _hiveDb.VerifyUser(request.UserId, request.Password);
        if (error != ErrorCode.None)  // 에러 코드가 None이 아닐 때 로그인 실패 처리
        {
            return new LoginResponse { Result = error }; // 에러 코드를 포함한 응답
        }

        var token = Security.MakeHashingToken(request.UserId, userNum);
        bool tokenSet = await _hiveRedis.SetTokenAsync(userNum, token, TimeSpan.FromHours(1));

        if (!tokenSet)
        {
            return new LoginResponse { Result = ErrorCode.InternalError };
        }

        return new LoginResponse { UserNum = userNum, HiveToken = token, Result = ErrorCode.None }; // 성공 응답
    }

}