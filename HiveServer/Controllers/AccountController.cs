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
    public async Task<IActionResult> Register([FromBody] AccountRequestDTO request)
    {
        _logger.LogInformation("Register attempt for email: {Email}", request.Email); // 정보 로깅

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            _logger.LogWarning("Invalid model state for email: {Email}. Errors: {Errors}", request.Email, string.Join(", ", errors)); // 경고 로깅
            return BadRequest(new AccountResponseDTO { Success = false, Message = string.Join("; ", errors) });
        }

        try
        {
            int userId = await _hiveDb.RegisterAccount(request.Email, request.Password);
            _logger.LogInformation("User registered successfully with ID: {UserId}", userId); // 성공 로그
            return Ok(new AccountResponseDTO { Success = true, Message = "User registered successfully"});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", request.Email); // 에러 로깅
            return StatusCode(500, new AccountResponseDTO { Success = false, Message = ex.Message });
        }
    }
}