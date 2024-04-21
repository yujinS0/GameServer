namespace HiveServer.Repository
{
    public interface IHiveRedis : IDisposable
    {
        Task<bool> SetTokenAsync(long playerId, string token, TimeSpan expiration);
        Task<bool> ValidateTokenAsync(long playerId, string token);
        // Task<string> GetTokenAsync(long playerId);
    }
}