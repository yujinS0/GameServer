namespace APIServer.Repository;

public interface IMemoryDb : IDisposable
{
    Task<string> SetUserTokenAsync(long userId);
    Task<bool> ValidateTokenAsync(string token, string userId);
}
