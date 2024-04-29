using CommandLine;
using Echoserver;
using System;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello SuperSocketLite");

            var serverOption = ParseCommandLine(args);
            if (serverOption == null)
            {
                return;
            }


            var server = new MainServer();
            server.InitConfig(serverOption);
            server.CreateServer(); 
            // --------------------------------------- 여기까지는 아직 서버가 실행되지는 않은 것 (start 부터 실행되는 것)

            var IsResult = server.Start(); // mserver class - super socket에 있는 것.
            // 스레드를 만들어서 동작하는 것이다. 따라서 종료하지 말라고 ~

            if (IsResult)
            {
                MainServer.MainLogger.Info("서버 네트워크 시작");
            }
            else
            {
                Console.WriteLine("서버 네트워크 시작 실패");
                return;
            }


            Console.WriteLine("key를 누르면 종료한다....");
            Console.ReadKey(); // 종료하지 말라고! 넣어둠!
            // 비동기라서 거의 멀티 스레드이다

            // stop으로 네트워크 동작 멈추고 완료할 작업 끝내고 종료하는 방식으로 구현할 수도 있다!
            //server.Destory();
        }

        static ServerOption ParseCommandLine(string[] args)
        {
            var option = new ServerOption
            {
                Port = 32452,
                MaxConnectionNumber = 32,
                Name = "EchoServer"
            };

            return option;
        }

    }

    public class ServerOption // 실행을 위해 정보를 파싱한 것
    {
        public int Port { get; set; }

        public int MaxConnectionNumber { get; set; } = 0;

        public string Name { get; set; }
    }

}
