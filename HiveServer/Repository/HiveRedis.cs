using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;

namespace HiveServer.Repository
{
    public class HiveRedis : IHiveRedis
    {
        private readonly RedisConnection _redisConn;
        private readonly ILogger<HiveRedis> _logger;

        public HiveRedis(IOptions<DbConfig> dbConfig, ILogger<HiveRedis> logger)
        {
            _logger = logger;
            RedisConfig config = new RedisConfig("default", dbConfig.Value.RedisHiveConnection);
            _redisConn = new RedisConnection(config);
        }

        public async Task<bool> SetTokenAsync(long userId, string token, TimeSpan expiration)
        {
            var key = $"user:token:{userId}";
            try
            {
                var redisString = new RedisString<string>(_redisConn, key, TimeSpan.FromHours(1));
                return await redisString.SetAsync(token, expiry: expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set token for player ID: {userId}", userId);
                return false;
            }
        }

        // public async Task<string> GetTokenAsync(long userId)
        // {
        //     var key = $"user:token:{userId}";
        //     try
        //     {
        //         var redisString = new RedisString<string>(_redisConn, key, TimeSpan.FromHours(1));
        //         var result = await redisString.GetAsync();
        //         return result.HasValue ? result.Value : null; //
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Failed to get token for player ID: {userId}", userId);
        //         return null; //
        //     }
        // }
        /// [질문] 현재 안쓰는 함수이긴 합니다.
        /// 위 주의 표시 반환값이 있는 함수에서 Possible Null Reference Return
        /// (제가 생각하는 해결방법)
        /// 1) Null 반환 허용 
        ///     // public async Task<string?> GetTokenAsync(long playerId)
        /// 2) Default 값을 반환하도록 -> 토큰의 경우 빈 문자열을 반환하도록 
        ///     return result.HasValue ? result.Value : string.Empty;
        /// -> 둘 중에 어떤 게 더 보편적인 방법인지 궁금합니다!

        public async Task<bool> ValidateTokenAsync(long userId, string token)
        {
            var key = $"user:token:{userId}";
            try
            {
                var redisString = new RedisString<string>(_redisConn, key, TimeSpan.FromHours(1));
                var result = await redisString.GetAsync();
                return result.HasValue && result.Value == token; // True if token exists and matches
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to validate token for player ID: {userId}", userId);
                return false;
            }
        }

        public void Dispose()
        {
            // _redisConn.Dispose(); 
        }
    }
}