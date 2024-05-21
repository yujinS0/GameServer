namespace HiveServer.Repository
{
    public interface IHiveDb : IDisposable
    {
        public Task<ErrorCode> RegisterAccount(string userId, string password);
        public Task<(ErrorCode, long)> VerifyUser(string userId, string password);
        Task<(ErrorCode, long)> GetUserNumByUserId(string userId);
        Task<(ErrorCode, string?)> GetUserIdByUserNum(long UserNum);    
    }
}
