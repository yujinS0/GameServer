using HiveServer.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("ConnectionStrings")); // DbConfig 설정 로드

builder.Services.AddScoped<IHiveDb, HiveDb>(); // hive mysql
builder.Services.AddSingleton<IHiveRedis, HiveRedis>(); // hive redis

// 로깅 설정
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();