﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OmokServer;
using System.Threading.Tasks;


namespace OmokServer;

class Program
{
    static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) => // configuration 설정
            {
                var env = hostingContext.HostingEnvironment;
                //config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureLogging(logging => // 로깅 설정
            {
                logging.SetMinimumLevel(LogLevel.Debug);
                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServerOption>(hostContext.Configuration.GetSection("ServerOption"));
                services.AddHostedService<MainServer>(); // 인터페이스를 구현하는 서비스를 애플리케이션의 서비스 컨테이너에 추가
                                                         // StartAsync: 호스트가 시작될 때 호출되며, 여기서 백그라운드 작업이나 다른 초기화 작업을 시작
                                                         // StopAsync: 호스트가 정상적으로 종료되기 전에 호출되며, 여기서 자원을 정리하거나 작업을 마무리
            })
            .Build();

        await host.RunAsync(); // 호스트 비동기적으로 시작, 실행 상태로 계속 유지
    }
}