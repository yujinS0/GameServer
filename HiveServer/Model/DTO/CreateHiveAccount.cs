using System;
using System.ComponentModel.DataAnnotations;
// using ServerCommon;

namespace HiveServer.Model.DTO
{
    public class AccountRequest
    {
        [Required]
        [EmailAddress]
        [MinLength(1, ErrorMessage = "EMAIL CANNOT BE EMPTY")]
        [StringLength(50, ErrorMessage = "EMAIL IS TOO LONG")]
        public required string Email { get; set; }  // 사용자 이메일

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [StringLength(30, ErrorMessage = "PASSWORD IS TOO LONG")]
        [DataType(DataType.Password)]
        public required string Password { get; set; } // 사용자 비밀번호
    }

    public class AccountResponse // [TODO] : Success 없애기. 그리고 Message 없애고 에러코드로 바꾸기 
    {
        public bool Success { get; set; }  // 요청 성공 여부
        public required string Message { get; set; }
    }
}
