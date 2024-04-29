using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks.Dataflow; // TPL

using CSBaseLib;

namespace ChatServer
{
    // PacketProcessor : 패킷 처리용 클래스 -> 여기서 스레드 1개 만들어서 패킷 처리
    // 프로세서가 패킷을 생성하고 처리하고 관리하는 클래스이다
    // 스레드 간 데이터 주고 받는 것은 BufferBlock이라는 TPL로 !
    class PacketProcessor
    {
        bool IsThreadRunning = false;
        System.Threading.Thread ProcessThread = null;

        //receive쪽에서 처리하지 않아도 Post에서 블럭킹 되지 않는다. 
        //BufferBlock<T>(DataflowBlockOptions) 에서 DataflowBlockOptions의 BoundedCapacity로 버퍼 가능 수 지정. BoundedCapacity 보다 크게 쌓이면 블럭킹 된다
        BufferBlock<ServerPacketData> MsgBuffer = new BufferBlock<ServerPacketData>(); // BufferBlock : 스레드간 데이터 주고 받는 것은

        UserManager UserMgr = new UserManager(); // 유저매니저

        Tuple<int,int> RoomNumberRange = new Tuple<int, int>(-1, -1);
        List<Room> RoomList = new List<Room>(); // 룸매니저

        Dictionary<int, Action<ServerPacketData>> PacketHandlerMap = new Dictionary<int, Action<ServerPacketData>>();
        PKHCommon CommonPacketHandler = new PKHCommon(); // 패킷처리 관련
        PKHRoom RoomPacketHandler = new PKHRoom();
        // [에코서버와 차이점]
        // 에코서버에는 PKHCommon가 패킷 처리(PacketDefine - PACKETID에) 1개만 있었는데, 이제 많아짐
        // 따라서 하나의 파일에 다 정의하면 너무 많아지니깐, 파일을 나눔
        // 패킷핸들러로 나눠서 얘를 상속 받는 패킷커먼과 패킷 룸 2개로..
            // 커먼 : 공통으로 처리해야하는 부분 (로직) 요청 처리
            // 룸 : 방과 관련된 로직 처리


        //TODO MainServer를 인자로 주지말고, func을 인자로 넘겨주는 것이 좋다
        public void CreateAndStart(List<Room> roomList, MainServer mainServer)
        {
            var maxUserCount = MainServer.ServerOption.RoomMaxCount * MainServer.ServerOption.RoomMaxUserCount;
            UserMgr.Init(maxUserCount);

            RoomList = roomList;
            var minRoomNum = RoomList[0].Number;
            var maxRoomNum = RoomList[0].Number + RoomList.Count() - 1;
            RoomNumberRange = new Tuple<int, int>(minRoomNum, maxRoomNum);
            
            RegistPacketHandler(mainServer);

            IsThreadRunning = true;
            ProcessThread = new System.Threading.Thread(this.Process); // 스레드 만들고 있음! - Process
            ProcessThread.Start();
        }
        
        public void Destory()
        {
            IsThreadRunning = false;
            MsgBuffer.Complete();
        }
              
        public void InsertPacket(ServerPacketData data) // 
        {
            MsgBuffer.Post(data); // 버퍼 블럭에 넣어주고 있음!
                             // TPL : 버퍼에 데이터 넣을 때는 Post로 넣음
                             // 뺄때는 receive
        }

        
        void RegistPacketHandler(MainServer serverNetwork)
        {            
            CommonPacketHandler.Init(serverNetwork, UserMgr);
            CommonPacketHandler.RegistPacketHandler(PacketHandlerMap);                
            
            RoomPacketHandler.Init(serverNetwork, UserMgr);
            RoomPacketHandler.SetRooomList(RoomList);
            RoomPacketHandler.RegistPacketHandler(PacketHandlerMap);
        }

        void Process()
        {
            while (IsThreadRunning) // 계속 실행~
            {
                //System.Threading.Thread.Sleep(64); //테스트 용
                try
                {
                    var packet = MsgBuffer.Receive(); // 이 부분이 버퍼에서 데이터 빼는 거 Receive 사용!
                    // 슈퍼소켓에서 receive 쪽에서 패킷 처리 스레드 적으로 데이터를 넘길 때 서로 주고 받는 부분
                    // c#의 TPL = 버퍼 블럭 (큐 같은 것)
                    // 스레드 세이프하게 데이터를 넣고 뺄 수 있음.
                    // receive 했을 때 아무것도 없으면 대기(stop)
                    // 데이터 넣을 때는 Post로 넣음 - InsertPacket 함수

                    // 이때 post와 receive 는 스레드 세이프 하기 때문에 Lock 필요 없
                    // -> TPL 의 장점이 바로 이것이다~!

                    if (PacketHandlerMap.ContainsKey(packet.PacketID)) // 받은 패킷ID가 PacketHandlerMap에 있는지?
                    {
                        PacketHandlerMap[packet.PacketID](packet); // 핸들러 실행
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("세션 번호 {0}, PacketID {1}, 받은 데이터 크기: {2}", packet.SessionID, packet.PacketID, packet.BodyData.Length);
                    }
                }
                catch (Exception ex)
                {
                    //IsThreadRunning.IfTrue(() => MainServer.MainLogger.Error(ex.ToString()));
                    if (IsThreadRunning)
                    {
                        MainServer.MainLogger.Error(ex.ToString());
                    }
                }
            }
        }


    }
}
