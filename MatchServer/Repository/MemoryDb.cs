using CloudStructures;
using Microsoft.Extensions.Options;
using CloudStructures.Structures;
using MatchServer.Model.DTO;

namespace MatchServer.Repository
{
    public class MemoryDb : IMemoryDb
    {
        private readonly RedisConnection _redisConn;
        private readonly ILogger<MemoryDb> _logger;

        public MemoryDb(IOptions<DbConfig> dbConfig, ILogger<MemoryDb> logger)
        {
            _logger = logger;
            RedisConfig config = new RedisConfig("default", dbConfig.Value.RedisGameConnection);
            _redisConn = new RedisConnection(config);
        }

        public async Task<RoomInfo> PopRoomInfoAsync()
        {
            var redisList = new RedisList<RoomInfo>(_redisConn, "RoomInfoList", null);
            var roomInfoResult = await redisList.RightPopAsync();
            var roomInfo = roomInfoResult.Value;

            if (roomInfo != null)
            {
                _logger.LogInformation("Popped RoomInfo {RoomNumber} from Redis.", roomInfo.RoomNumber);
            }
            else
            {
                _logger.LogWarning("No RoomInfo available in Redis.");
            }

            return roomInfo;
        }

        public void Dispose()
        {
            // _redisConn?.Dispose(); // Redis 연결 해제
        }
        /// [질문]
        /// CloudStructures 사용하면 내부적으로 연결 처리 해주니깐 Dispose 안해줘도 괜찮은지 궁금합니다.
        ///
    }
}