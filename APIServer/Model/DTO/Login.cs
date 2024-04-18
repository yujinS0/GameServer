using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace APIServer.DTO; // [TODO] ErrorCode 사용하도록 수정

public class LoginRequest
{
    [Required]
    public long UserId { get; set; }

    [Required]
    public string HiveToken { get; set; }
}

public class LoginResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public string Token { get; set; } = "";
    [Required] public int Uid { get; set; } = 0;

    // public DataLoadUserInfo userData { get; set; }
}

public class TokenValidationRequest
{
    [Required] public string UserID { get; set; }
    [Required] public string HiveToken { get; set; }
}

public class TokenValidationResponse
{
    [JsonPropertyName("success")] // 오류로 고생했던 부분.. 애초에 message 안 쓰면 해결될 문제
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}