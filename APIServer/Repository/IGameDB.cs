using APIServer.Models.GameDB;

namespace APIServer.Repository
{
    public interface IGameDb : IDisposable
    {
        Task<UserGameData> CreateUserGameDataAsync(long userNum, string UserId);
        Task<UserGameData> GetUserGameDataAsync(long userNum);
    }
}
