using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks.Dataflow;


namespace OmokServer;

class PacketProcessor
{
    bool _isThreadRunning = false;
    System.Threading.Thread _processThread = null;

    public Func<string, byte[], bool> NetSendFunc;
    
    //receive쪽에서 처리하지 않아도 Post에서 블럭킹 되지 않는다. 
    //BufferBlock<T>(DataflowBlockOptions) 에서 DataflowBlockOptions의 BoundedCapacity로 버퍼 가능 수 지정. BoundedCapacity 보다 크게 쌓이면 블럭킹 된다
    BufferBlock<MemoryPackBinaryRequestInfo> _msgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();

    UserManager _userMgr = new UserManager();

    RoomManager _roomMgr;

    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _packetHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();
    PKHCommon _commonPacketHandler = new PKHCommon();
    PKHRoom _roomPacketHandler = new PKHRoom();
            
    public void CreateAndStart(RoomManager roomManager, ServerOption serverOpt) // 서버 옵션을 받아서 초기화
    {
        _roomMgr = roomManager;
        var maxUserCount = serverOpt.RoomMaxCount * serverOpt.RoomMaxUserCount;
        _userMgr.Init(maxUserCount);

        var roomList = _roomMgr.GetRoomsList();
        if (roomList.Count > 0)
        {
            var minRoomNum = roomList[0].Number;
            var maxRoomNum = roomList[roomList.Count - 1].Number;
        }

        RegistPacketHandler(); // 패킷 핸들러 등록

        _isThreadRunning = true;
        _processThread = new System.Threading.Thread(this.Process);
        _processThread.Start();
    }
    
    public void Destory()
    {
        MainServer.MainLogger.Info("PacketProcessor::Destory - begin");

        _isThreadRunning = false;
        _msgBuffer.Complete();

        _processThread.Join();

        MainServer.MainLogger.Info("PacketProcessor::Destory - end");
    }
          
    public void InsertPacket(MemoryPackBinaryRequestInfo data)
    {
        _msgBuffer.Post(data); // 버퍼 블럭에 넣기
    }

    
    void RegistPacketHandler() // 패킷 핸들러 등록
    {
        PKHandler.NetSendFunc = NetSendFunc;
        PKHandler.DistributeInnerPacket = InsertPacket;
        _commonPacketHandler.Init(_userMgr, _roomMgr);
        _commonPacketHandler.RegistPacketHandler(_packetHandlerMap);     
        
        _roomPacketHandler.Init(_userMgr, _roomMgr);
        _roomPacketHandler.SetRooomList(_roomMgr.GetRoomsList());
        _roomPacketHandler.RegistPacketHandler(_packetHandlerMap);

        Room.DistributeInnerPacket = InsertPacket; // 룸에서 이너패킷 사용을 위해 추가한 코드
    }

    void Process()
    {
        while (_isThreadRunning)
        {
            try
            {
                var packet = _msgBuffer.Receive();

                var header = new MemoryPackPacketHeadInfo();
                header.Read(packet.Data);

                if (_packetHandlerMap.ContainsKey(header.Id))
                {
                    _packetHandlerMap[header.Id](packet);
                }
            }
            catch (Exception ex)
            {
                if (_isThreadRunning)
                {
                    MainServer.MainLogger.Error(ex.ToString());
                }
            }
        }
    }
}
