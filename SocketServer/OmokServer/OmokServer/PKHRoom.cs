using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;


namespace OmokServer;

public class PKHRoom : PKHandler
{
    List<Room> _roomList = null;
    int _startRoomNumber;
    
    public void SetRooomList(List<Room> roomList)
    {
        _roomList = roomList;
        _startRoomNumber = roomList[0].Number;
    }

    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> packetHandlerMap)
    {
        packetHandlerMap.Add((int)PACKETID.ReqRoomEnter, RequestRoomEnter);
        packetHandlerMap.Add((int)PACKETID.ReqRoomLeave, RequestLeave);
        packetHandlerMap.Add((int)PACKETID.NtfInRoomLeave, NotifyLeaveInternal);
        packetHandlerMap.Add((int)PACKETID.ReqRoomChat, RequestChat);

        //
        packetHandlerMap.Add((int)PACKETID.ReqReadyOmok, ReqReadyOmok);


        // 오목 게임 관련
        packetHandlerMap.Add((int)PACKETID.ReqPutMok, RequestPlaceStone);
        //packetHandlerMap.Add((int)PACKETID.ResPutMok, ResponsePlaceStone);
        //packetHandlerMap.Add((int)PACKETID.NTFPutMok, NotifyPlaceStone);

        packetHandlerMap.Add((int)PACKETID.NTFEndOmok, NotifyGameEnd);
    }


    Room GetRoom(int roomNumber)
    {
        var index = roomNumber - _startRoomNumber;

        if( index < 0 || index >= _roomList.Count())
        {
            return null;
        }

        return _roomList[index];
    }
            
    (bool, Room, RoomUser) CheckRoomAndRoomUser(string userNetSessionID)
    {
        var user = _userMgr.GetUser(userNetSessionID);
        if (user == null)
        {
            return (false, null, null);
        }

        var roomNumber = user.RoomNumber;
        var room = GetRoom(roomNumber);

        if(room == null)
        {
            return (false, null, null);
        }

        var roomUser = room.GetUserByNetSessionId(userNetSessionID);

        if (roomUser == null)
        {
            return (false, room, null);
        }

        return (true, room, roomUser);
    }

    public void RequestRoomEnter(MemoryPackBinaryRequestInfo packetData) 
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("RequestRoomEnter");

        try
        {
            var user = _userMgr.GetUser(sessionID); // 유저 정보 가져오기

            if (user == null || user.IsConfirm(sessionID) == false) // 유저 정보가 없거나 세션 아이디가 일치하지 않으면
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_USER, sessionID);
                return;
            }

            if (user.IsStateRoom()) // 이미 방에 들어가 있는 상태이면
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_STATE, sessionID); 
                return;
            }

            var reqData = MemoryPackSerializer.Deserialize<PKTReqRoomEnter>(packetData.Data); 
            
            var room = GetRoom(reqData.RoomNumber); // 방 정보 가져오기

            if (room == null) 
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_ROOM_NUMBER, sessionID);
                return;
            }

            if (room.AddUser(user.ID(), sessionID) == false) 
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_FAIL_ADD_USER, sessionID);
                return;
            }


            user.EnteredRoom(reqData.RoomNumber); // 유저가 방에 들어갔다고 표시

            room.NotifyPacketUserList(sessionID); // 방에 있는 유저 리스트 전송
            room.NofifyPacketNewUser(sessionID, user.ID()); // 새로운 유저에게 방에 있는 유저 리스트 전송

            ResponseEnterRoomToClient(ERROR_CODE.NONE, sessionID); // 방 입장 성공

            MainServer.MainLogger.Debug("RequestEnterInternal - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }

    void ResponseEnterRoomToClient(ERROR_CODE errorCode, string sessionID)
    {
        var resRoomEnter = new PKTResRoomEnter()
        {
            Result = (short)errorCode
        };

        var sendPacket = MemoryPackSerializer.Serialize(resRoomEnter);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.ResRoomEnter);
        
        NetSendFunc(sessionID, sendPacket);
    }

    public void RequestLeave(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("방나가기 요청 받음");

        try
        {
            var user = _userMgr.GetUser(sessionID);
            if(user == null)
            {
                return;
            }

            if(LeaveRoomUser(sessionID, user.RoomNumber) == false)
            {
                return;
            }

            user.LeaveRoom();

            ResponseLeaveRoomToClient(sessionID);

            MainServer.MainLogger.Debug("Room RequestLeave - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }

    bool LeaveRoomUser(string sessionID, int roomNumber)
    {
        MainServer.MainLogger.Debug($"LeaveRoomUser. SessionID:{sessionID}");

        var room = GetRoom(roomNumber);
        if (room == null)
        {
            return false;
        }

        var roomUser = room.GetUserByNetSessionId(sessionID);
        if (roomUser == null)
        {
            return false;
        }
                    
        var userID = roomUser.UserID;
        room.RemoveUser(roomUser);

        room.NotifyPacketLeaveUser(userID);
        return true;
    }

    void ResponseLeaveRoomToClient(string sessionID)
    {
        var resRoomLeave = new PKTResRoomLeave()
        {
            Result = (short)ERROR_CODE.NONE
        };

        var sendPacket = MemoryPackSerializer.Serialize(resRoomLeave);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.ResRoomLeave);
   
        NetSendFunc(sessionID, sendPacket);
    }

    public void NotifyLeaveInternal(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug($"NotifyLeaveInternal. SessionID: {sessionID}");

        var reqData = MemoryPackSerializer.Deserialize<PKTInternalNtfRoomLeave>(packetData.Data);            
        LeaveRoomUser(sessionID, reqData.RoomNumber);
    }
            
    public void RequestChat(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("Room RequestChat");

        try
        {
            var roomObject = CheckRoomAndRoomUser(sessionID);

            if(roomObject.Item1 == false)
            {
                return;
            }


            var reqData = MemoryPackSerializer.Deserialize<PKTReqRoomChat>(packetData.Data);

            var notifyPacket = new PKTNtfRoomChat()
            {
                UserID = roomObject.Item3.UserID,
                ChatMessage = reqData.ChatMessage
            };

            var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
            MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfRoomChat);
            
            roomObject.Item2.Broadcast("", sendPacket);

            MainServer.MainLogger.Debug("Room RequestChat - Success");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }
    // ReqReadyOmok 함수 
    public void ReqReadyOmok(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("ReqReadyOmok");

        try
        {
            var user = _userMgr.GetUser(sessionID); // 유저 정보 가져오기
            if (user == null || user.IsConfirm(sessionID) == false) // 유저 정보가 없거나 세션 아이디가 일치하지 않으면
            {
                ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_USER, sessionID);
                return;
            }
            var room = GetRoom(user.RoomNumber);
            if (room == null)
            {
                MainServer.MainLogger.Error("Room not found for the user");
                return;
            }

            var roomUser = room.GetUserByNetSessionId(sessionID);
            roomUser.SetReady();

            NotifyReadyOmok(ERROR_CODE.NONE, sessionID); 

            MainServer.MainLogger.Debug("ReqReadyOmok - Success");

            // Check if all users are ready
            if (room.AreAllUsersReady())
            {
                room.StartGame();
            }
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }

    void NotifyReadyOmok(ERROR_CODE errorCode, string sessionID)
    {
        var user = _userMgr.GetUser(sessionID);
        //var room = GetRoom(user.RoomNumber);
        var roomUser = (GetRoom(user.RoomNumber)).GetUserByNetSessionId(sessionID);

        var notifyPacket = new PKTNtfReadyOmok()
        {
            UserID = user.ID(),
            IsReady = roomUser.GetIsReady()
        };

        var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfReadyOmok);

        NetSendFunc(sessionID, sendPacket);
    }

    // RequestPlaceStone 함수: 클라이언트로부터 오목 돌 두기 요청을 받아 처리
    public void RequestPlaceStone(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("RequestPlaceStone");

        try
        {
            var roomObject = CheckRoomAndRoomUser(sessionID);
            if (!roomObject.Item1) // 유효하지 않은 요청 처리
            {
                MainServer.MainLogger.Error("Invalid room or user.");
                return;
            }
            int StoneColor = roomObject.Item3.StoneColor;

            var reqData = MemoryPackSerializer.Deserialize<PKTReqPutMok>(packetData.Data);
          

            // 게임 로직 처리: 돌 두기
            bool result = roomObject.Item2.game.PlaceStone(reqData.PosX, reqData.PosY, StoneColor);
            if (!result)
            {
                MainServer.MainLogger.Error("Failed to place stone.");
                return;
            }

            // NotifyPlaceStone
            var notifyPacket = new PKTNtfPutMok()
            {
                PosX = reqData.PosX,
                PosY = reqData.PosY,
                Mok = StoneColor
            };

            var sendPacket = MemoryPackSerializer.Serialize(notifyPacket);
            MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NTFPutMok);
            roomObject.Item2.Broadcast(sessionID, sendPacket); // 해당 방의 모든 유저에게 통보

            MainServer.MainLogger.Debug("Stone placed successfully.");
        }
        catch (Exception ex)
        {
            MainServer.MainLogger.Error(ex.ToString());
        }
    }



    void NotifyPlaceStone(ERROR_CODE errorCode, string sessionID)
    {
        //var sessionID = packetData.SessionID;

        MainServer.MainLogger.Debug("NotifyPlaceStone");
    }

    public void NotifyGameEnd(MemoryPackBinaryRequestInfo packetData)
    {
        var sessionID = packetData.SessionID;
        MainServer.MainLogger.Debug("NotifyGameEnd");
    }

}
