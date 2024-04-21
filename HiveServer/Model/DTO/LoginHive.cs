using System.ComponentModel.DataAnnotations;

namespace HiveServer.Model.DTO
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
        [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        [Required]
        public ErrorCode Result { get; set; } = ErrorCode.None;

        [Required]
        public long UserId { get; set; }
        public string HiveToken { get; set; }
    }

}
