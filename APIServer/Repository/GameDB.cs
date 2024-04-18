using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using APIServer.Models;
using APIServer.Models.GameDB;
using APIServer.Repository;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace APIServer.Services;
public class GameDb : IGameDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<GameDb> _logger;
    private MySqlConnection _connection;
    private QueryFactory _queryFactory;

    public GameDb(IOptions<DbConfig> dbConfig, ILogger<GameDb> logger)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        _connection = new MySqlConnection(_dbConfig.Value.MysqlGameDBConnection);
        _connection.Open();

        // Configure QueryFactory
        var compiler = new MySqlCompiler();
        _queryFactory = new QueryFactory(_connection, compiler);
    }

    public async Task<UserGameData> CreateUserGameDataAsync(string userId)
    {
        var newUser = new UserGameData
        {
            UserId = userId,
            Level = 1,
            Exp = 0,
            Win = 0,
            Lose = 0
        };

        await _queryFactory.Query("UserGameData").InsertAsync(newUser);

        return newUser;
    }

    public async Task<UserGameData> GetUserGameDataAsync(string userId)
    {
        var user = await _queryFactory.Query("UserGameData").Where("UserId", "=", userId).FirstOrDefaultAsync<UserGameData>();

        return user;
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
        _connection = null;
    }
}