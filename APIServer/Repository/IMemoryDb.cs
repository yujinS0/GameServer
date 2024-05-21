namespace APIServer.Repository;

public interface IMemoryDb : IDisposable
{
    Task<string> SetUserTokenAsync(long userNum);
    Task<bool> ValidateTokenAsync(string token, string userId);
}
