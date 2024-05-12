namespace HiveServer.Repository
{
    public interface IHiveDb : IDisposable
    {
        public Task<ErrorCode> RegisterAccount(string email, string password);
        public Task<(ErrorCode, long)> VerifyUser(string email, string password);
        Task<(ErrorCode, long)> GetUserIdByEmail(string email);
        Task<(ErrorCode, string?)> GetEmailByUserId(long userId);    
    }
}
