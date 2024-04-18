using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using APIServer.Repository;
using APIServer.Services;
using CloudStructures;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// DbConfig 설정 로드
builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("ConnectionStrings"));

// mysql 서비스 등록
builder.Services.AddScoped<IGameDb, GameDb>();

// Redis configuration [TODO] 이것도 위 바꾼 코드처럼 인터페이스 활용해서 객체 만드는 방식으로 수정
var redisConfiguration = builder.Configuration.GetConnectionString("RedisGameConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration)); 

builder.Services.AddSingleton<IMemoryDb, MemoryDb>(); // 수정?

// HttpClientFactory 추가
builder.Services.AddHttpClient();

// 로깅 설정
builder.Logging.ClearProviders(); // 기본 로깅 프로바이더를 제거합니다.
builder.Logging.AddConsole(); // 콘솔 로거를 추가합니다.
builder.Logging.SetMinimumLevel(LogLevel.Information); // 최소 로깅 레벨을 Information으로 설정합니다.


builder.Services.AddControllers(); 

var app = builder.Build(); 

app.UseRouting();

app.MapControllers(); 

app.Run(); 