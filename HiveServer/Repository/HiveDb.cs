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

        public async Task<ErrorCode> RegisterAccount(string userId, string password)
        {
            try
            {
                var salt = Security.SaltString();
                var hashedPassword = Security.MakeHashingPassWord(salt, password);

                var id = await _queryFactory.Query("account").InsertGetIdAsync<int>(new {
                    UserId = userId,
                    Password = hashedPassword,
                    Salt = salt
                });

                _logger.LogInformation($"Account successfully registered with ID: {id}.");
                return ErrorCode.None; // Success
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error when registering account with UserId: {UserId}", userId);
                return ErrorCode.DatabaseError;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register account with UserId: {UserId}", userId);
                return ErrorCode.InternalError; // Generic error for unexpected issues
            }
        }

        public async Task<(ErrorCode, long)> VerifyUser(string userId, string password)
        {
            try
            {
                var query = _queryFactory.Query("account")
                                         .Select("userNum")
                                         .WhereRaw("userId = ? AND password = SHA2(CONCAT(salt, ?), 256)", userId, password);

                var userNum = await query.FirstOrDefaultAsync<long?>();

                if (userNum.HasValue)
                {
                    _logger.LogInformation("User verified successfully with ID: {UserNum}", userNum.Value);
                    return (ErrorCode.None, userNum.Value);
                }
                return (ErrorCode.UserNotFound, 0);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error when verifying user with UserId: {UserId}", userId);
                return (ErrorCode.DatabaseError, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify user with UserId: {UserId}", userId);
                return (ErrorCode.InternalError, 0);
            }
        }
        public async Task<(ErrorCode, long)> GetUserNumByUserId(string userId)
        {
            try
            {
                var userNum = await _queryFactory.Query("account")
                                                .Where("userId", userId)
                                                .Select("userNum")
                                                .FirstOrDefaultAsync<long?>();

                if (userNum.HasValue)
                {
                    _logger.LogInformation("Retrieved UserNum {UserNum} for UserId: {UserId}", userNum.Value, userId);
                    return (ErrorCode.None, userNum.Value);
                }
                return (ErrorCode.UserNotFound, 0);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error when retrieving UserNum for UserId: {UserId}", userId);
                return (ErrorCode.DatabaseError, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve UserNum for UserId: {UserId}", userId);
                return (ErrorCode.InternalError, 0);
            }
        }

        public async Task<(ErrorCode, string?)> GetUserIdByUserNum(long userNum)
        {
            try
            {
                var userId = await _queryFactory.Query("account")
                                            .Where("userNum", userNum)
                                            .Select("userId")
                                            .FirstOrDefaultAsync<string>();

                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("Retrieved UserId {UserId} for UserNum: {UserNum}", userId, userNum);
                    return (ErrorCode.None, userId);
                }
                return (ErrorCode.UserNotFound, null);
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Database error when retrieving UserId for UserNum: {UserNum}", userNum);
                return (ErrorCode.DatabaseError, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve UserId for UserNum: {UserNum}", userNum);
                return (ErrorCode.InternalError, null);
            }
        }

    }
}