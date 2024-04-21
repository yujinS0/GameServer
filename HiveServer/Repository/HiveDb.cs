using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using HiveServer.Services;
using Microsoft.Extensions.Options;

namespace HiveServer.Repository
{
    public class HiveDb : IHiveDb
    {
        private readonly IOptions<DbConfig> _dbConfig;
        private readonly ILogger<HiveDb> _logger; // 로거의 제네릭 타입도 HiveDb로 변경 -> why?
        private MySqlConnection _connection;
        readonly QueryFactory _queryFactory;

        public HiveDb(IOptions<DbConfig> dbConfig, ILogger<HiveDb> logger) // 생성자
        {
            _dbConfig = dbConfig;
            _logger = logger;

            _connection = new MySqlConnection(_dbConfig.Value.MysqlHiveDBConnection); // 읽어와서 연결 
            _connection.Open();

            _queryFactory = new QueryFactory(_connection, new MySqlCompiler()); // QueryFactory 객체 생성
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            // _connection = null; // 
        }

        public async Task<ErrorCode> RegisterAccount(string email, string password)
        {
            try
            {
                var salt = Security.SaltString();
                var hashedPassword = Security.MakeHashingPassWord(salt, password);

                var id = await _queryFactory.Query("account").InsertGetIdAsync<int>(new {
                    Email = email,
                    Password = hashedPassword,
                    Salt = salt
                });

                _logger.LogInformation($"Account successfully registered with ID: {id}.");
                return ErrorCode.None; // Success
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error when registering account with Email: {Email}", email);
                return ErrorCode.DatabaseError;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register account with Email: {Email}", email);
                return ErrorCode.InternalError; // Generic error for unexpected issues
            }
        }

        public async Task<(ErrorCode, long)> VerifyUser(string email, string password)
        {
            try
            {
                var query = _queryFactory.Query("account")
                                         .Select("userid")
                                         .WhereRaw("email = ? AND password = SHA2(CONCAT(salt, ?), 256)", email, password);

                var userId = await query.FirstOrDefaultAsync<long?>();

                if (userId.HasValue)
                {
                    _logger.LogInformation("User verified successfully with ID: {UserId}", userId.Value);
                    return (ErrorCode.None, userId.Value);
                }
                return (ErrorCode.UserNotFound, 0);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error when verifying user with Email: {Email}", email);
                return (ErrorCode.DatabaseError, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify user with Email: {Email}", email);
                return (ErrorCode.InternalError, 0);
            }
        }
    }
}