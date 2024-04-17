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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid) // [TODO] ModelState 볼 필요 없음. 입력받을 때 걸러주니깐! 
        {
            return BadRequest(ModelState); // [TODO] Badrequest 필요 없음.(클라이언트에서 잘못 접근했을 때만 주로 쓰고) 이것도 에러 코드로 수정하기 !! 
        }
        try
        {
            var playerId = await _hiveDb.VerifyUser(request.Email, request.Password);
            if (playerId == 0)  // playerId가 0이면 로그인 실패
            {
                _logger.LogWarning("Login failed for Email: {Email}, invalid credentials", request.Email);
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid credentials",
                    userId = 0,
                    HiveToken = ""
                });
            }

            var token = Security.CreateAuthToken();
            var db = _redis.GetDatabase(); // [TODO] DI의 의미가 없다 이렇게 하면, memoryDb를 만들어서 처리해야한다. 오직 함수만 호출하는 형식으로 
            // 로그인 토큰 저장 1시간으로 설정
            await db.StringSetAsync($"user:{playerId}", token, TimeSpan.FromHours(1));  // 레디스에 토큰값 만들어서 저장하게 하는 함수 호출하는 방식으로 

            return Ok(new LoginResponse
            {
                Success = true, // 
                Message = "User logged in successfully.",
                userId = playerId,
                HiveToken = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login attempt failed for Email: {Email}", request.Email);
            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = "Login attempt failed due to an internal error",
                userId = 0,
                HiveToken = ""
            });
        }
    }
    
}
