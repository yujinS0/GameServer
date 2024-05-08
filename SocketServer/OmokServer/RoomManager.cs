using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmokServer;

public class RoomManager
{
    List<Room> _roomsList = new List<Room>();
    //private System.Timers.Timer _roomCheckTimer;
    //private System.Timers.Timer _turnCheckTimer;

    int _checkRoomInterval = 0;
    int _checkRoomIndex = 0;
    int _maxRoomCount = 0;

    //int checkTurn = 30000; // 30초

    public void CreateRooms(ServerOption serverOpt)
    {
        var maxRoomCount = serverOpt.RoomMaxCount;
        var startNumber = serverOpt.RoomStartNumber;
        var maxUserCount = serverOpt.RoomMaxUserCount;

        _checkRoomInterval = 20000; //serverOpt.RoomIntervalMilliseconds;
        _maxRoomCount = 25; // serverOpt.maxRoomCount;
        _checkRoomIndex = 0;

        for (int i = 0; i < maxRoomCount; ++i)
        {
            var roomNumber = (startNumber + i);
            var room = new Room();
            room.Init(i, roomNumber, maxUserCount);

            _roomsList.Add(room);
        }
        //_roomCheckTimer.Interval = _checkRoomInterval;
        //_roomCheckTimer.Elapsed += NotifyEvent;
        //_roomCheckTimer.Start();
        //MainServer.MainLogger.Debug("_roomCheckTimer 타이머 시작!");

    }
    public List<Room> GetRoomsList()
    {
        return _roomsList;
    }

    //private void NotifyEvent(object sender, System.Timers.ElapsedEventArgs e)
    //{
    //    //var memoryPakcPacket = new MemoryPackBinaryRequestInfo(null);
    //    //memoryPakcPacket.Data = new byte[MemoryPackPacketHeadInfo.HeadSize];
    //    //MemoryPackPacketHeadInfo.WritePacketId(memoryPakcPacket.Data, (UInt16)PACKETID.NtfInCheckRoom);
    //    //var packetData = new PKTNtfInCheckRoom() { };
    //    //PacketProcessor.InsertPacket(packetData);  // PacketProcessor의 큐에 패킷 추가
    //    var packet = new PKTNtfInCheckRoom();

    //    var packetData = MemoryPackSerializer.Serialize(packet);
    //    MemoryPackPacketHeadInfo.Write(packetData, PACKETID.NtfInCheckRoom);

    //    var memoryPakcPacket = new MemoryPackBinaryRequestInfo(null);
    //    memoryPakcPacket.Data = packetData;

    //    DistributeInnerPacket(memoryPakcPacket);
    //    MainServer.MainLogger.Debug("NotifyEvent NtfInCheckRoom 이너 패킷 보냄 = 룸 상태 검사");
    //    //PacketProcessor.InsertPacket(memoryPakcPacket);  // PacketProcessor의 큐에 패킷 추가
    //}

    public void CheckRoom()
    {
        if (_checkRoomIndex >= _roomsList.Count)
        {
            _checkRoomIndex = 0;
        }
        int EndIndex = _checkRoomIndex + _maxRoomCount;
        if(EndIndex > _roomsList.Count)
        {
            EndIndex = _roomsList.Count;
        }
        MainServer.MainLogger.Debug("==CheckRoom 정상 진입");
        for (int i = _checkRoomIndex; i < EndIndex; i++)
        {
            DateTime cutTime = DateTime.Now;
            // 룸체크
            _roomsList[i].RoomCheck(cutTime); 

            
            // 턴체크
            _roomsList[i].TurnCheck(cutTime);
        }
        _checkRoomIndex += _maxRoomCount;
    }

    public void StartTurnTimer()
    {

    }

    //public void InitTurnTimer()
    //{
    //    _turnCheckTimer = new System.Timers.Timer(TurnDuration);
    //    _turnCheckTimer.AutoReset = false;
    //    _turnCheckTimer.Elapsed += HandleTurnTimeout;
    //}

    //private void HandleTurnTimeout(object? sender, ElapsedEventArgs e)
    //{
    //    var packet = new PKTNtfInCheckTurn();

    //    var packetData = MemoryPackSerializer.Serialize(packet);
    //    MemoryPackPacketHeadInfo.Write(packetData, PACKETID.NtfInCheckTurn);

    //    var memoryPakcPacket = new MemoryPackBinaryRequestInfo(null);
    //    memoryPakcPacket.Data = packetData;

    //    DistributeInnerPacket(memoryPakcPacket);
    //    MainServer.MainLogger.Debug("NotifyEvent NtfInCheckTurn 이너 패킷 보냄 = 턴검사");
    //}
}
