using System.Collections.Concurrent;
using MatchServer.Model.DTO;
using MatchServer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MatchServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchController : ControllerBase
{
    private static ConcurrentQueue<string> _reqQueue = new();
    private static ConcurrentDictionary<string, RoomInfo> _completeDic = new();

    private readonly ILogger<MatchController> _logger;
    private readonly IMemoryDb _memoryDb;

    public MatchController(ILogger<MatchController> logger, IMemoryDb memoryDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;
    }

    [HttpPost("request")]
    public async Task<IActionResult> Match([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/request : {request.UserId}", request.UserId);
        if (request == null || string.IsNullOrEmpty(request.UserId))
        {
            _logger.LogError("Invalid match request data.");
            return BadRequest(new MatchResponse { Result = ErrorCode.InvalidRequest });
        }

        _reqQueue.Enqueue(request.UserId);
        _logger.LogInformation("Added {UserId} to match request queue.", request.UserId);

        if (_reqQueue.Count >= 2)
        {
            var roomInfo = await _memoryDb.PopRoomInfoAsync();
            if (roomInfo != null)
            {
                if (_reqQueue.TryDequeue(out string UserId1) && _reqQueue.TryDequeue(out string UserId2))
                {
                    _completeDic[UserId1] = roomInfo;
                    _completeDic[UserId2] = roomInfo;
                    _logger.LogInformation("Matched {UserId1} and {UserId2} in room {RoomNum}.", UserId1, UserId2, roomInfo.RoomNumber);
                }
            }
        }

        return Ok(new MatchResponse { Result = ErrorCode.None });
    }

    [HttpPost("ismatched")]
    public IActionResult IsMatched([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/ismatched : {request.UserId}", request.UserId);

        if (request == null || string.IsNullOrEmpty(request.UserId))
        {
            _logger.LogError("Invalid ismatched request data.");
            return BadRequest(new MatchCompleteResponse
            {
                Result = ErrorCode.InvalidRequest,
                Success = 0,
                UserId = request.UserId,
                RoomNum = -1,
                ServerAddress = ""
            });
        }

        if (_completeDic.TryGetValue(request.UserId, out RoomInfo roomInfo))
        {
            // 매칭된 이메일과 룸 번호 쌍을 딕셔너리에서 제거
            _completeDic.TryRemove(request.UserId, out _);
            return Ok(new MatchCompleteResponse
            {
                Result = ErrorCode.None,
                Success = 1,
                UserId = request.UserId,
                RoomNum = roomInfo.RoomNumber,
                ServerAddress = roomInfo.ServerAddress
            });
        }

        return Ok(new MatchCompleteResponse
        {
            Result = ErrorCode.None,
            Success = 0,
            UserId = request.UserId,
            RoomNum = -1,
            ServerAddress = ""
        });
    }

    [HttpPost("cancel")]
    public IActionResult CancelMatch([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/cancel : {request.UserId}", request.UserId);

        if (request == null || string.IsNullOrEmpty(request.UserId))
        {
            _logger.LogError("Invalid cancel match request data.");
            return BadRequest(new MatchCancelResponse
            {
                Result = ErrorCode.InvalidRequest,
                Message = "Invalid request data."
            });
        }

        if (_completeDic.ContainsKey(request.UserId))
        {
            _logger.LogError("Cannot cancel match for {UserId} as it is already completed.", request.UserId);
            return BadRequest(new MatchCancelResponse
            {
                Result = ErrorCode.MatchNotFound,
                Message = "Match already completed. Cannot cancel."
            });
        }

        var tempQueue = new ConcurrentQueue<string>();
        bool removed = false; // 매칭 요청 큐에서 요청 제거 여부

        while (_reqQueue.TryDequeue(out string userId))
        {
            if (userId == request.UserId && !removed)
            {
                removed = true;
                _logger.LogInformation("Removed {UserId} from match request queue.", request.UserId);
            }
            else
            {
                tempQueue.Enqueue(userId);
            }
        }

        _reqQueue = tempQueue;

        if (!removed)
        {
            return Ok(new MatchCancelResponse
            {
                Result = ErrorCode.MatchNotFound,
                Message = "No matching request found to cancel."
            });
        }

        return Ok(new MatchCancelResponse
        {
            Result = ErrorCode.None,
            Message = "Match request cancelled successfully."
        });
    }
}