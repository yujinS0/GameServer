using HiveServer.Repository;
using StackExchange.Redis; // [TODO] CloudStructures 라이브러리 사용하기 

var builder = WebApplication.CreateBuilder(args);

// DbConfig 설정 로드
builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("ConnectionStrings"));

// mysql 서비스 등록
builder.Services.AddScoped<IHiveDb, HiveDb>();

// Redis configuration // [TODO] 이것도 위 바꾼 코드처럼 인터페이스 활용해서 객체 만드는 방식으로 수정
var redisConfiguration = builder.Configuration.GetConnectionString("RedisHiveConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration)); 
// builder.Services.AddSingleton<IHiveRedis, HiveRedis>();

// 로깅 설정
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // 콘솔 로거
builder.Logging.SetMinimumLevel(LogLevel.Information); // 최소 로깅 레벨 Information


builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();