using StackExchange.Redis; // [TODO] CloudStructures 라이브러리 사용으로 변경하기
using System;
using System.Threading.Tasks;

namespace APIServer.Repository
{
    public class MemoryDb : IMemoryDb
    {
        private readonly IConnectionMultiplexer _redis;

        public MemoryDb(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<string> SetUserTokenAsync(string userId)
        {
            var db = _redis.GetDatabase();
            var token = Guid.NewGuid().ToString(); // 새로운 토큰 생성
            await db.StringSetAsync(userId, token, TimeSpan.FromHours(2)); // 토큰 2시간 동안 유효

            return token;
        }

        public void Dispose()
        {
            _redis?.Dispose(); // Redis 연결 해제
        }
    }
}