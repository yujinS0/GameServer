using APIServer.Models.GameDB;

namespace APIServer.Repository
{
    public interface IGameDb : IDisposable
    {
        Task<UserGameData> CreateUserGameDataAsync(string userId);
        Task<UserGameData> GetUserGameDataAsync(string userId);
    }
}
