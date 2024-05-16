using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using APIServer.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchController : ControllerBase
{
    private static ConcurrentQueue<string> _reqQueue = new();
    private static ConcurrentQueue<int> _room = new();
    private static ConcurrentDictionary<string, int> _completeDic = new();

    private readonly ILogger<MatchController> _logger;

    public MatchController(ILogger<MatchController> logger)
    {
        _logger = logger;

        // 초기화된 방 번호 큐 (_room)에 예시로 방 번호 추가
        for (int i = 99; i > 0; i--)
        {
            _room.Enqueue(i);
        }
    }

    [HttpPost("request")]
    public IActionResult Match([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/request : {request.Email}", request.Email);
        if (request == null || string.IsNullOrEmpty(request.Email))
        {
            _logger.LogError("Invalid match request data.");
            return BadRequest(new MatchResponse { Result = ErrorCode.InvalidRequest });
        }

        _reqQueue.Enqueue(request.Email);   
        _logger.LogInformation("Added {Email} to match request queue.", request.Email);

        if (_reqQueue.Count >= 2)
        {
            if (_room.TryDequeue(out int roomNum))
            {
                if (_reqQueue.TryDequeue(out string email1) && _reqQueue.TryDequeue(out string email2))
                {
                    _completeDic[email1] = roomNum;
                    _completeDic[email2] = roomNum;
                    _logger.LogInformation("Matched {Email1} and {Email2} in room {RoomNum}.", email1, email2, roomNum);
                }
            }
        }

        return Ok(new MatchResponse { Result = ErrorCode.None });
    }

    [HttpPost("ismatched")]
    public IActionResult IsMatched([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/ismatched : {request.Email}", request.Email);

        if (request == null || string.IsNullOrEmpty(request.Email))
        {
            _logger.LogError("Invalid ismatched request data.");
            return BadRequest(new MatchCompleteResponse
            {
                Result = ErrorCode.InvalidRequest,
                Success = 0,
                Email = request.Email,
                RoomNum = -1
            });
        }

        if (_completeDic.TryGetValue(request.Email, out int roomNum))
        {
            // 매칭된 이메일과 룸 번호 쌍을 딕셔너리에서 제거
            _completeDic.TryRemove(request.Email, out _); 
            return Ok(new MatchCompleteResponse
            {
                Result = ErrorCode.None,
                Success = 1,
                Email = request.Email,
                RoomNum = roomNum
            });
        }

        return Ok(new MatchCompleteResponse
        {
            Result = ErrorCode.None,
            Success = 0,
            Email = request.Email,
            RoomNum = -1
        });
    }

    [HttpPost("cancel")]
    public IActionResult CancelMatch([FromBody] MatchRequest request)
    {
        _logger.LogInformation($"POST match/cancel : {request.Email}", request.Email);

        if (request == null || string.IsNullOrEmpty(request.Email))
        {
            _logger.LogError("Invalid cancel match request data.");
            return BadRequest(new MatchCancelResponse
            {
                Result = ErrorCode.InvalidRequest,
                Message = "Invalid request data."
            });
        }

        if (_completeDic.ContainsKey(request.Email))
        {
            _logger.LogError("Cannot cancel match for {Email} as it is already completed.", request.Email);
            return BadRequest(new MatchCancelResponse
            {
                Result = ErrorCode.MatchNotFound,
                Message = "Match already completed. Cannot cancel."
            });
        }

        var tempQueue = new ConcurrentQueue<string>();
        bool removed = false; // 매칭 요청 큐에서 요청 제거 여부

        while (_reqQueue.TryDequeue(out string email))
        {
            if (email == request.Email && !removed)
            {
                removed = true;
                _logger.LogInformation("Removed {Email} from match request queue.", request.Email);
            }
            else
            {
                tempQueue.Enqueue(email);
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