namespace HiveServer.Repository
{
    public interface IHiveDb : IDisposable
    {
        public Task<ErrorCode> RegisterAccount(string email, string password);
        public Task<(ErrorCode, long)> VerifyUser(string email, string password);
    }
}
