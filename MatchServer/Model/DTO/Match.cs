using System.ComponentModel.DataAnnotations;

namespace MatchServer.Model.DTO
{
    public class MatchRequest
    {
        [Required] public string Email { get; set; }
    }

    public class MatchResponse
    {
        [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    }

    public class MatchCompleteResponse
    {
        [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
        [Required] public int Success { get; set; } // 매칭 성공하면 1
        [Required] public string Email { get; set; }
        [Required] public int RoomNum { get; set; }
        [Required] public string ServerAddress { get; set; }
    }
    public class MatchCancelResponse
    {
        [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
        [Required] public string Message { get; set; }
    }

    public class RoomInfo
    {
        public int RoomNumber { get; set; }
        public string ServerAddress { get; set; }

        public RoomInfo(int roomNumber, string serverAddress)
        {
            RoomNumber = roomNumber;
            ServerAddress = serverAddress; // "localhost:32451"
        }
    }
}
