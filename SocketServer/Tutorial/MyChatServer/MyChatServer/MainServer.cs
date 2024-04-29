﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;

using CSBaseLib;


//TODO 1. 주기적으로 접속한 세션이 패킷을 주고 받았는지 조사(좀비 클라이언트 검사)

namespace ChatServer
{
    public class MainServer : AppServer<ClientSession, EFBinaryRequestInfo>
    {
        public static ChatServerOption ServerOption;
        public static SuperSocket.SocketBase.Logging.ILog MainLogger;

        SuperSocket.SocketBase.Config.IServerConfig m_Config;

        PacketProcessor MainPacketProcessor = new PacketProcessor();
        RoomManager RoomMgr = new RoomManager();
        
        
        public MainServer()
            : base(new DefaultReceiveFilterFactory<ReceiveFilter, EFBinaryRequestInfo>())
        {
            NewSessionConnected += new SessionHandler<ClientSession>(OnConnected);
            SessionClosed += new SessionHandler<ClientSession, CloseReason>(OnClosed);
            NewRequestReceived += new RequestHandler<ClientSession, EFBinaryRequestInfo>(OnPacketReceived);
        }

        public void InitConfig(ChatServerOption option)
        {
            ServerOption = option;

            m_Config = new SuperSocket.SocketBase.Config.ServerConfig
            {
                Name = option.Name,
                Ip = "Any",
                Port = option.Port,
                Mode = SocketMode.Tcp,
                MaxConnectionNumber = option.MaxConnectionNumber,
                MaxRequestLength = option.MaxRequestLength,
                ReceiveBufferSize = option.ReceiveBufferSize,
                SendBufferSize = option.SendBufferSize
            };
        }
        
        public void CreateStartServer() // 에코서버는 여기서 객체 생성과 시작까지 처리하는 중
        {
            try
            {
                bool bResult = Setup(new SuperSocket.SocketBase.Config.RootConfig(), m_Config, logFactory: new NLogLogFactory());

                if (bResult == false)
                {
                    Console.WriteLine("[ERROR] 서버 네트워크 설정 실패 ㅠㅠ");
                    return;
                } 
                else 
                {
                    MainLogger = base.Logger;
                    MainLogger.Info("서버 초기화 성공");
                }


                CreateComponent(); // 차이점! 채팅서버에서 사용할 객체들을 만들고 있음

                Start(); // 슈퍼소켓의 start !

                MainLogger.Info("서버 생성 성공");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 서버 생성 실패: {ex.ToString()}");
            }          
        }

        
        public void StopServer()
        {            
            Stop();

            MainPacketProcessor.Destory();
        }

        public ERROR_CODE CreateComponent() // 채팅서버에서 사용할 객체들을 만들고 있음
        {
            Room.NetSendFunc = this.SendData; // 그래서 룸 객체에서 NetSendFunc를 호출하면 결론적으로 SendData 이 자식이 호출
            RoomMgr.CreateRooms(); // RoomMgr = Room Mananger = 룸 객체 관리
                // 처음부터 룸 객체를 만들어두고 재활용하고 있음

            // 패킷 처리를 스레드 1개에서 진행 
            MainPacketProcessor = new PacketProcessor(); 
            MainPacketProcessor.CreateAndStart(RoomMgr.GetRoomsList(), this);

            MainLogger.Info("CreateComponent - Success");
            return ERROR_CODE.NONE;
        }

        public bool SendData(string sessionID, byte[] sendData) // 룸 객체에서 NetSendFunc를 호출 시 얘가 호출되는 거임.
        { // 실제로 전송하는 과정
            var session = GetSessionByID(sessionID);

            try
            {
                if (session == null)
                {
                    return false;
                }

                session.Send(sendData, 0, sendData.Length); // 해당 세션 객체에 sendData.Length 데이터 보내기
            }
            catch(Exception ex)
            {
                // TimeoutException 예외가 발생할 수 있다
                MainServer.MainLogger.Error($"{ex.ToString()},  {ex.StackTrace}");

                session.SendEndWhenSendingTimeOut(); 
                session.Close();
            }
            return true;
        }

        public void Distribute(ServerPacketData requestPacket)
        {
            MainPacketProcessor.InsertPacket(requestPacket); // InsertPacket 함수 호출!
        }
                        
        void OnConnected(ClientSession session) // 에코서버와 다른 점!
                                                // ->  패킷처리를 여기서 다 하는 게 아니라, 
                                                //     패킷 프로세서 쪽(패킷 프로세서가 만든 스레드)으로 전달하도록
                                                //     Distribute 함수를 호출하고 있다.
        {
            //옵션의 최대 연결 수를 넘으면 SuperSocket이 바로 접속을 짤라버린다. 즉 이 OnConneted 함수가 호출되지 않는다
            MainLogger.Info(string.Format("세션 번호 {0} 접속", session.SessionID));
                        
            var packet = ServerPacketData.MakeNTFInConnectOrDisConnectClientPacket(true, session.SessionID);            
            Distribute(packet); // Distribute 함수 호출
        }

        void OnClosed(ClientSession session, CloseReason reason)
        {
            MainLogger.Info(string.Format("세션 번호 {0} 접속해제: {1}", session.SessionID, reason.ToString()));

            var packet = ServerPacketData.MakeNTFInConnectOrDisConnectClientPacket(false, session.SessionID);
            Distribute(packet); // Distribute 함수 호출
        }

        void OnPacketReceived(ClientSession session, EFBinaryRequestInfo reqInfo) // 외부에서 오는 패킷 처리 -> 여기서 체크하고 Message 버퍼에 넣음
        {
            /// 슈퍼소켓의 네트워크 이벤트 = OnReceive (데이터 온 거)
            /// 만약 여기서 패킷 처리를 하면, 멀티스레드가 된다!
            /// 그래서 만약 여기서 처리할거면, 
            /// UserMananger 같은 공유 객체에 접근할 때 마다 락을 걸어야함
            /// -> 따라서 우리의 경우에는 PacketProcessor 에서 처리하고 있음
            ////////
            // 그래서 지금 여기서는 패킷이 오면, 패킷의 정보를 ServerPacketData에 넣고있음
            // [질문] 의문.
            // 왜 여기서.. ServerPacketData를 사용하는가요?????
            // 외부에서 오는 패킷 처리는 PacketData 에서 하는 거 아닌가?

            MainLogger.Debug(string.Format("세션 번호 {0} 받은 데이터 크기: {1}, ThreadId: {2}", session.SessionID, reqInfo.Body.Length, System.Threading.Thread.CurrentThread.ManagedThreadId));

            var packet = new ServerPacketData(); // 패킷 정보 넣는 중
            packet.SessionID = session.SessionID; // 
            packet.PacketSize = reqInfo.Size;            
            packet.PacketID = reqInfo.PacketID;
            packet.Type = reqInfo.Type;
            packet.BodyData = reqInfo.Body;
                    
            Distribute(packet);
        }
    }

    public class ClientSession : AppSession<ClientSession, EFBinaryRequestInfo>
    {
    }
    // 앱세션을 상속받는 클라이언트 세션
    // - 앱세션에 세션아이디가 존재함
    // - 세션아이디를 슈퍼소켓에게 알려줘야지,
    //    어떤 세션 객체에 send를 해야할 지 알 수가 있음!
    //     -> 따라서 UserManager에서 SessionID 관리하고 있음! 

        /*class ConfigTemp
        {
            static public List<string> RemoteServers = new List<string>();
        }*/
 }
