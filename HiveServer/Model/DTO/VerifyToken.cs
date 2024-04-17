using System.ComponentModel.DataAnnotations;

namespace HiveServer.Model.DTO
{
    public class VerifyTokeRequestDTO
    {
        [Required]
        public string HiveToken { get; set; }
        [Required]
        public long UserId { get; set; }
    }

    public class VerifyTokenResponseDTO
    {
        public bool Success { get; set; }  // 요청 성공 여부
        public required string Message { get; set; } // 결과 메시지
    }
}
