using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using HiveServer.Model.DTO;
using HiveServer.Model.DAO;
using HiveServer.Repository;

namespace HiveServer.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    // [TODO] 컨트롤러에서 오류를 처리할 필요는 없을 것 
    // 오류 관련해서 지금 메세지로 처리하고 있는데, 이상함... 그냥 에러 코드 내보내게 하기 ! 
    // 컨트롤러에서 그냥 에러코드만 ..
    // 구현X / 에러만 던지고
    ///.......... errorcode = none 이게 성공이다! -> 이거 코드 자료 참고해서 수정 
    // 실제 구현될 코드는 Repository에 가 있어야함

    // private readonly DatabaseService _databaseService;
    private readonly IHiveDb _hiveDb;
    private readonly ILogger<AccountController> _logger; // 로거 인스턴스 추가

    // 생성자 + 인스턴스 초기화
    public AccountController(IHiveDb hiveDb, ILogger<AccountController> logger)
    {
        _hiveDb = hiveDb;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountRequest request) 
    // [TODO] 여기서 Task<> 저 괄호 안에 지금 그냥 반환타입 AccountRequest를 내보내면 된다
    // 굳이 지금 IActionResult할 필요가 없음 (이건 그냥 Veiw 있다는 가정상 이렇게 많이 쓰기 때문임 )
    // 로깅 관련해서도 level 설정에 대한 고민 해봐야함! 
    {
        _logger.LogInformation("Register attempt for email: {Email}", request.Email); // 정보 로깅

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            _logger.LogWarning("Invalid model state for email: {Email}. Errors: {Errors}", request.Email, string.Join(", ", errors)); // 경고 로깅
            return BadRequest(new AccountResponse { Success = false, Message = string.Join("; ", errors) });
        }

        try
        {
            int userId = await _hiveDb.RegisterAccount(request.Email, request.Password);
            _logger.LogInformation("User registered successfully with ID: {UserId}", userId); // 성공 로그
            return Ok(new AccountResponse { Success = true, Message = "User registered successfully"});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", request.Email); // 에러 로깅
            return StatusCode(500, new AccountResponse { Success = false, Message = ex.Message });
        }
    }
}