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
}