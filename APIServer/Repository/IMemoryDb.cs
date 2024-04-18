namespace APIServer.Repository;

public interface IMemoryDb : IDisposable
{
    Task<string> SetUserTokenAsync(string userId);
}
