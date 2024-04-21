namespace APIServer.Repository;

public interface IMemoryDb : IDisposable
{
    Task<string> SetUserTokenAsync(long userId);
}
