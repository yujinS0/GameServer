using System.ComponentModel.DataAnnotations;

namespace HiveServer.Model.DTO
{
    public class VerifyTokeRequest
    {
        [Required]
        public string HiveToken { get; set; }
        [Required]
        public long UserId { get; set; }
    }

    public class VerifyTokenResponse // [TODO] 형태 Success/Message 에서 에러코드로 바꾸기!! 
    {
        public bool Success { get; set; }  // 요청 성공 여부
        public required string Message { get; set; } // 결과 메시지
    }
}
