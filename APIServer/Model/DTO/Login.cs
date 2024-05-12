using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIServer.Models.GameDB;


namespace APIServer.DTO;

public class LoginRequest
{
    [Required]
    public long UserID { get; set; }
    public required string Email { get; set; }
    public required string HiveToken { get; set; }
}

public class LoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public string Token { get; set; } = "";
    [Required] public long Uid { get; set; } = 0;
    public UserGameData UserGameData {get; set;}
}

public class TokenValidationRequest
{
    [Required] public long UserID { get; set; }
    public required string HiveToken { get; set; }
}

public class TokenValidationResponse
{
    [Required] public ErrorCode Result {get; set;} = ErrorCode.None;
}