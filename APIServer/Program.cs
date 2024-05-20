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
using APIServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("ConnectionStrings")); // DbConfig 설정 로드

builder.Services.AddScoped<IGameDb, GameDb>(); // Game Mysql
builder.Services.AddSingleton<IMemoryDb, MemoryDb>(); // Game Redis

builder.Services.AddHttpClient(); // HttpClientFactory 추가

// 로깅 설정
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers(); 

var app = builder.Build(); 

app.UseRouting();

// 사용자 토큰 검증용 미들웨어 추가
app.UseMiddleware<TokenValidationMiddleware>();

app.MapControllers(); 

app.Run(); 