using System;

namespace ChatServer
{
    // 전반적으로 에코 서버와 동일!
    class Program
    {
        //dotnet ChatServer.dll --uniqueID 1 --roomMaxCount 16 --roomMaxUserCount 4 --roomStartNumber 1 --maxUserCount 100
        static void Main(string[] args)
        {
            var serverOption = ParseCommandLine(args);
            if(serverOption == null)
            {
                return;
            }

           
            var serverApp = new MainServer();
            serverApp.InitConfig(serverOption);

            serverApp.CreateStartServer(); // !Create 하는 부분 -> CreateStartServer 보기!
            // 에코서버는 CreateStartServer() 이후에 server.Start() 로 서버 시작했는데, 여기는 다르다!
            // 채팅서버는 CreateStartServer()에서 create 객체 생성도 하고 시작도 하고 있음.

            MainServer.MainLogger.Info("Press q to shut down the server");

            while (true)
            {
                System.Threading.Thread.Sleep(50);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.KeyChar == 'q')
                    {
                        Console.WriteLine("Server Terminate ~~~");
                        serverApp.StopServer();
                        break;
                    }
                }
                                
            }
        }

        static ChatServerOption ParseCommandLine(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<ChatServerOption>(args) as CommandLine.Parsed<ChatServerOption>;

            if (result == null)
            {
                System.Console.WriteLine("Failed Command Line");
                return null;
            }

            return result.Value;
        }                  

    } // end Class
}
