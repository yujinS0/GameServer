using System.ComponentModel.DataAnnotations;

namespace HiveServer.Model.DTO
{
    public class AccountRequest
    {
        [Required]
        [EmailAddress]
        [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
        [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
        public required string UserId { get; set; }

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

    public class UserNumRequest 
    {
        public long UserNum { get; set; }
    }

    public class UserIdResponse 
    {
        public string UserId { get; set; }
        public ErrorCode Result { get; set; } = ErrorCode.None;
    }
}
