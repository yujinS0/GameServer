using System;
using System.Threading.Tasks;

namespace HiveServer.Repository
{
    public interface IHiveDb : IDisposable
    {
        public Task<IEnumerable<dynamic>> GetAllAccounts();
        public Task<int> RegisterAccount(string email, string password);
        public Task<long> VerifyUser(string email, string password);
    }
}
