using Microsoft.AspNetCore.Mvc;
using HiveServer.Model.DTO;
using HiveServer.Repository;

namespace HiveServer.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IHiveDb _hiveDb;
    private readonly ILogger<AccountController> _logger; // 로거 인스턴스 추가

    public AccountController(IHiveDb hiveDb, ILogger<AccountController> logger)
    {
        _hiveDb = hiveDb;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<AccountResponse> Register([FromBody] AccountRequest request) 
    {
        AccountResponse response = new();

        response.Result = await _hiveDb.RegisterAccount(request.UserId, request.Password);

        return response;
    }

    [HttpPost("userId")]
    public async Task<ActionResult<UserIdResponse>> GetUserIdByUserNum([FromBody] UserNumRequest request)
    {
        var (errorCode, userId) = await _hiveDb.GetUserIdByUserNum(request.UserNum);

        if (errorCode == ErrorCode.None && userId != null)
        {
            return Ok(new UserIdResponse { UserId = userId, Result = ErrorCode.None });
        }
        else if (errorCode == ErrorCode.UserNotFound)
        {
            _logger.LogError("User not found for UserNum: {UserNum}", request.UserNum);
            return NotFound(new UserIdResponse { Result = ErrorCode.UserNotFound });
        }
        else
        {
            _logger.LogError("Internal server error while retrieving userId for UserNum: {UserNum}", request.UserNum);
            return StatusCode(500, new UserIdResponse { Result = ErrorCode.InternalError });
        }
    }

}