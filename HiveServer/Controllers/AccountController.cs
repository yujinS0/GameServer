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

        response.Result = await _hiveDb.RegisterAccount(request.Email, request.Password);

        return response;
    }

    [HttpPost("email")]
    public async Task<ActionResult<UserEmailResponse>> GetEmailByUserId([FromBody] UserIdRequest request)
    {
        var (errorCode, email) = await _hiveDb.GetEmailByUserId(request.UserId);

        if (errorCode == ErrorCode.None && email != null)
        {
            return Ok(new UserEmailResponse { Email = email, Result = ErrorCode.None });
        }
        else if (errorCode == ErrorCode.UserNotFound)
        {
            _logger.LogError("User not found for UserId: {UserId}", request.UserId);
            return NotFound(new UserEmailResponse { Result = ErrorCode.UserNotFound });
        }
        else
        {
            _logger.LogError("Internal server error while retrieving email for UserId: {UserId}", request.UserId);
            return StatusCode(500, new UserEmailResponse { Result = ErrorCode.InternalError });
        }
    }

}