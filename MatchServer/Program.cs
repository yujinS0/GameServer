using MatchServer.Repository;
using MatchServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("ConnectionStrings")); // DbConfig ���� �ε�

builder.Services.AddSingleton<IMemoryDb, MemoryDb>(); // Game Redis

builder.Services.AddHttpClient(); // HttpClientFactory �߰�

// �α� ����
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();