using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using System.Diagnostics;

using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;
using EchoServer;
using NLog;

namespace Echoserver
{
    class MainServer : AppServer<NetworkSession, EFBinaryRequestInfo> // MainServer class에서 실질적인 네트워크 작업 처리
                    // AppServer가 super socket의 핵심 본체  // NetworkSession : 새로운 연결이 발생할 때 세션이 생김 (논리적인 개념)
    {
        public static SuperSocket.SocketBase.Logging.ILog MainLogger; // Log
        IServerConfig m_Config;

        // 패킷 헨들러 함수 등록을 위한 변수 선언부 (우리는 echo 서버라 없어도 괜찮지만, 공부를 위해 생성)
        Dictionary<int, Action<NetworkSession, EFBinaryRequestInfo>> HandlerMap = new Dictionary<int, Action<NetworkSession, EFBinaryRequestInfo>>();
                // 특정 패킷 id (int)가 호출될 때, 실행하기 위한 딕셔너리
        CommonHandler CommonHan = new CommonHandler(); // 딕셔너리에 연결될 함수들을 선언하는 클래스

        public MainServer()
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, EFBinaryRequestInfo>())
        { // delegate
            NewSessionConnected += new SessionHandler<NetworkSession>(OnConnected); // 연결이 시작되었을 떄 OnConnected 함수를 호출해줘라.
            SessionClosed += new SessionHandler<NetworkSession, CloseReason>(OnClosed);
            NewRequestReceived += new RequestHandler<NetworkSession, EFBinaryRequestInfo>(RequestReceived);
        }

        public void InitConfig(ServerOption option) // super socket 동작을 위한 설정 정보 생성
        {
            m_Config = new ServerConfig // ServerConfig : super socket에 존재하는 config 파일 
            {
                Port = option.Port, // 포트번호
                Ip = "Any", // "Any" : 기본 ip 주소
                MaxConnectionNumber = option.MaxConnectionNumber, //  최대 동시 접속자 수
                Mode = SocketMode.Tcp, // 소켓모드
                Name = option.Name // 생략 가능 - 서버 이름
            };
        }

        public void CreateServer() // super socket 을 실행시키는 부분
        {
            try
            {
                bool bResult = Setup(new RootConfig(), m_Config, logFactory: new NLogLogFactory()); // 먼저 setup 설정! (config 파일 정보가 잘 들어가야 함) 
                                    // RootConfig() : super socket 에서만 사용 (lite 에서는 무시하기)
                if (bResult == false)
                {
                    Console.WriteLine("[ERROR] 서버 네트워크 설정 실패 ㅠㅠ");
                    return;
                }
                else
                {
                    MainLogger = base.Logger; // 슈퍼소켓에서의 로그 객체를 echo 서버에서도 사용할 것이기 때문에 연결
                }

                RegistHandler();

                MainLogger.Info("서버 생성 성공");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 서버 생성 실패: {ex.ToString()}");
            }
        }
        public bool IsRunning(ServerState eCurState) // 그냥 기본적으로 만든 함수 
        {
            if (eCurState == ServerState.Running)
            {
                return true;
            }

            return false;
        }


        void OnConnected(NetworkSession session)
        {
            MainLogger.Info($"세션 번호 {session.SessionID} 접속");
        }

        void OnClosed(NetworkSession session, CloseReason reason)
        {
            MainLogger.Info($"세션 번호 {session.SessionID} 접속해제: {reason.ToString()}");
        }

        void RequestReceived(NetworkSession session, EFBinaryRequestInfo reqInfo)
        {
            //MainLogger.Info($"세션 번호 {session.SessionID}, 받은 데이터 크기: {reqInfo.Body.Length}");

            var PacketID = reqInfo.PacketID;

            if (HandlerMap.ContainsKey(PacketID))
            {
                HandlerMap[PacketID](session, reqInfo); // super socket에서 주는 session과 reqlnfo를 그대로 넘겨주고 있음!!
                                                        // echo server 이므로 함수를 1개만 등록 -> 실제 server는 사용하는 모든 함수와 아이디를 등록
                                                        // [질문] 여기서 무슨 함수를 등록하는 거였지? 다시 보기!!!!!
            }
            else
            {
                MainLogger.Info($"세션 번호 {session.SessionID}, 받은 데이터 크기: {reqInfo.Body.Length}");
            }
        }
        void RegistHandler() // 딕셔너리 맵에 함수를 등록시키는 부분
        {
            HandlerMap.Add((int)PACKETID.REQ_ECHO, CommonHan.RequestEcho);

            MainLogger.Info("핸들러 등록 완료");
        }

    }

    

    public class NetworkSession : AppSession<NetworkSession, EFBinaryRequestInfo> // 새로운 연결이 발생할 때 세션 생김 -> super socket의 AppSession 상속
    { // 필요에 따라 내용 추가
    }
}
