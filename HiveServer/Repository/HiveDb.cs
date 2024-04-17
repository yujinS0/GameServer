using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using HiveServer.Services;

namespace HiveServer.Repository
{
    public class HiveDb : IHiveDb
    {
        private readonly ILogger<HiveDb> _logger; // 로거의 제네릭 타입도 HiveDb로 변경
        private readonly string _connectionString;
        private MySqlConnection _connection; // 데이터베이스 연결을 관리

        public HiveDb(string connectionString, ILogger<HiveDb> logger) // 생성자
        {
            _connectionString = connectionString;
            _logger = logger;
            _connection = new MySqlConnection(_connectionString); // 연결 초기화
            _connection.Open(); // 연결 열기
        }

        public void Dispose()
        {
            _connection?.Close(); // 연결이 열려있으면 닫기
            _connection?.Dispose(); // 연결 자원 해제
            _connection = null; // 참조 제거
        }

        public async Task<IEnumerable<dynamic>> GetAllAccounts()
        {
            var compiler = new MySqlCompiler();
            var queryFactory = new QueryFactory(_connection, compiler);
            _logger.LogInformation("Fetching all accounts from the database.");
            return await queryFactory.Query("account").GetAsync();
        }

        public async Task<int> RegisterAccount(string email, string password)
        {
            try
            {
                var salt = Security.SaltString(); // 솔트 생성
                var hashedPassword = Security.MakeHashingPassWord(salt, password); // 비밀번호 해싱

                var compiler = new MySqlCompiler();
                var queryFactory = new QueryFactory(_connection, compiler);
                var id = await queryFactory.Query("account").InsertGetIdAsync<int>(new {
                    Email = email,
                    Password = hashedPassword, // 해시된 비밀번호 저장
                    Salt = salt  // 솔트 저장
                });

                _logger.LogInformation($"Account successfully registered with ID: {id}, using hashed password and salt.");
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register account with Email: {Email}", email);
                throw;
            }
        }

        public async Task<long> VerifyUser(string email, string password)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT userid FROM account WHERE email = @Email AND password = SHA2(CONCAT(salt, @Password), 256)";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                var result = await cmd.ExecuteScalarAsync();
                if (result != null)
                {
                    return Convert.ToInt64(result); // 사용자 ID 반환
                }
            }
            return 0; // 일치하는 사용자가 없으면 0 반환
        }
    }
}