using System.ComponentModel.DataAnnotations;

namespace HiveServer.Model.DTO
{
    public class VerifyTokenRequest
    {
        [Required]
        public required string HiveToken { get; set; }
        [Required]
        public long UserId { get; set; }
    }

    public class VerifyTokenResponse
    {
        [Required]
        public ErrorCode Result { get; set; } = ErrorCode.None;
    }
}
