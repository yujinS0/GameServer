using APIServer.Models.GameDB;

namespace APIServer.Repository
{
    public interface IGameDb : IDisposable
    {
        Task<UserGameData> CreateUserGameDataAsync(long userId);
        Task<UserGameData> GetUserGameDataAsync(long userId);
    }
}
