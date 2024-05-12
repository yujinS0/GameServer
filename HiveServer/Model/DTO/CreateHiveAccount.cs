using System.ComponentModel.DataAnnotations;

namespace HiveServer.Model.DTO
{
    public class AccountRequest
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

    public class AccountResponse 
    {
        [Required]
        public ErrorCode Result { get; set; } = ErrorCode.None;
    }

    public class UserIdRequest 
    {
        public long UserId { get; set; }
    }

    public class UserEmailResponse 
    {
        public string Email { get; set; }
        public ErrorCode Result { get; set; } = ErrorCode.None;
    }
}
