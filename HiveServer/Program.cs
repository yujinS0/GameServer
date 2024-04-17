using HiveServer.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 데이터베이스 서비스 등록
// builder.Services.AddScoped<DatabaseService>(provider =>
//     new DatabaseService(builder.Configuration.GetConnectionString("MysqlHiveDBConnection")));
builder.Services.AddScoped<IHiveDb>(provider =>
    new HiveDb(
        builder.Configuration.GetConnectionString("MysqlHiveDBConnection"),
        provider.GetRequiredService<ILogger<HiveDb>>()
    ));

// Redis configuration
var redisConfiguration = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));


// 로깅 설정
builder.Logging.ClearProviders(); // 기본 로깅 프로바이더를 제거합니다.
builder.Logging.AddConsole(); // 콘솔 로거를 추가합니다.
builder.Logging.SetMinimumLevel(LogLevel.Information); // 최소 로깅 레벨을 Information으로 설정합니다.


builder.Services.AddControllers();

var app = builder.Build();

// API 엔드포인트 설정
app.MapControllers();

app.Run();