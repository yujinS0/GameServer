using CloudStructures;
using Microsoft.Extensions.Options;
using CloudStructures.Structures;

namespace APIServer.Repository
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
        public async Task<string> SetUserTokenAsync(long userId)
        {           
            var key = $"user:token:{userId}";
            var token = Guid.NewGuid().ToString(); // 새 토큰 생성
            var redisString = new RedisString<string>(_redisConn, key, TimeSpan.FromHours(1)); // RedisString 인스턴스 생성
            
            bool isSet = await redisString.SetAsync(token, TimeSpan.FromHours(1)); // 토큰 저장, 성공 여부 체크
            if (isSet)
            {
                return token;
            }
            else
            {
                throw new Exception("Failed to set the user token in Redis.");
            }
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