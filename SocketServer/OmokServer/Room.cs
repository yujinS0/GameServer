﻿using MemoryPack;
using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;


namespace OmokServer;

public class Room
{
    public const int InvalidRoomNumber = -1;

    public int Index { get; private set; }
    public int Number { get; private set; }
    int _maxUserCount = 0;

    List<RoomUser> _userList = new List<RoomUser>();

    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;

    public Game game;
    public DateTime TurnTime { get; set; }
    public DateTime StartTime { get; set; }

    public void Init(int index, int number, int maxUserCount)
    {
        Index = index;
        Number = number;
        _maxUserCount = maxUserCount;
    }

    public bool AddUser(string userID, string netSessionID)
    {
        if(GetUser(userID) != null)
        {
            return false;
        }

        var roomUser = new RoomUser();
        roomUser.Set(userID, netSessionID);
        _userList.Add(roomUser);

        return true;
    }

    public void RemoveUser(string netSessionID)
    {
        var index = _userList.FindIndex(x => x.NetSessionID == netSessionID);
        _userList.RemoveAt(index);
    }

    public bool RemoveUser(RoomUser user)
    {
        return _userList.Remove(user);
    }

    public RoomUser GetUser(string userID)
    {
        return _userList.Find(x => x.UserID == userID);
    }

    public RoomUser GetUserByNetSessionId(string netSessionID)
    {
        return _userList.Find(x => x.NetSessionID == netSessionID);
    }

    public int CurrentUserCount()
    {
        return _userList.Count();
    }

    public void NotifyPacketUserList(string userNetSessionID)
    {
        var packet = new PKTNtfRoomUserList();
        foreach (var user in _userList)
        {
            packet.UserIDList.Add(user.UserID);
        }

        // use MemoryPack
        var sendPacket = MemoryPackSerializer.Serialize(packet); // 직렬화
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfRoomUserList);
        
        NetSendFunc(userNetSessionID, sendPacket);
    }

    public void NofifyPacketNewUser(string newUserNetSessionID, string newUserID) // 새로운 유저가 들어왔을 때
    {
        var packet = new PKTNtfRoomNewUser();
        packet.UserID = newUserID;
        
        var sendPacket = MemoryPackSerializer.Serialize(packet); // 직렬화
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfRoomNewUser);
        
        Broadcast(newUserNetSessionID, sendPacket);
    }

    public void NotifyPacketLeaveUser(string userID)
    {
        if(CurrentUserCount() == 0)
        {
            return;
        }

        var packet = new PKTNtfRoomLeaveUser();
        packet.UserID = userID;
        
        var sendPacket = MemoryPackSerializer.Serialize(packet);
        MemoryPackPacketHeadInfo.Write(sendPacket, PACKETID.NtfRoomLeaveUser);
      
        Broadcast("", sendPacket);
    }

    public void Broadcast(string excludeNetSessionID, byte[] sendPacket)
    {
        foreach(var user in _userList)
        {
            if(user.NetSessionID == excludeNetSessionID)
            {
                continue;
            }

            NetSendFunc(user.NetSessionID, sendPacket);
        }
    }

    public bool AreAllUsersReady()
    {
        return _userList.All(user => user.GetIsReady());
    }

    public void StartGame()
    {
        if (game == null)
        {
            game = new Game(_userList, NetSendFunc);
            game.StartGame();
        }
    }

    internal void TurnCheck(DateTime cutTime) // 턴체크
    {
        //MainServer.MainLogger.Debug("==TurnCheck(DateTime cutTime) 턴체크 진입");

        if (game == null || !game.IsGameStarted) return;

        if ((cutTime - TurnTime).TotalSeconds > 2.5) // TODO : 이거 범위 맞는지? 그리고 Config로 받아오기
        {
            MainServer.MainLogger.Debug("==시간 초과로 턴 변경");
            // 턴 변경 패킷을 보낼 로직
            foreach (var user in _userList)
            {
                var sessionID = user.NetSessionID;
                var packet = new PKTNtfChangeTurn {};
                var sendData = MemoryPackSerializer.Serialize(packet);
                MemoryPackPacketHeadInfo.Write(sendData, PACKETID.NtfChangeTurn);
                NetSendFunc(sessionID, sendData);
            }
            game.SetTurnSkipCount1();
            game.IsGameTurnSkip6times();
            // 둔 시각 저장
            TurnTime = DateTime.Now;
            MainServer.MainLogger.Debug($"턴 넘긴 시각 저장 , TurnTime : {TurnTime}");
        }
    }

    internal void RoomCheck(DateTime cutTime) // 룸체크
    {
        //MainServer.MainLogger.Debug("==RoomCheck(DateTime cutTime) 룸체크 진입");

        if (!(_userList.Any())) { return; } // 유저가 없으면 체크 X
        if ((cutTime - StartTime).TotalMinutes > 30) // 30분
        {
            MainServer.MainLogger.Debug("==시간 초과로 접속 종료");

            foreach (var user in _userList)
            {
                var sessionID = user.NetSessionID;
                // 접속 종료 이너 패킷 보내기 
                var internalPacket = InnerPakcetMaker.MakeNTFInnerRoomLeavePacket(sessionID, this.Number, user.UserID);
                DistributeInnerPacket(internalPacket);
            }
        }
    }
}

public class RoomUser
{
    public string UserID { get; private set; }
    public string NetSessionID { get; private set; }
    public bool IsReady { get; private set; }
    public int StoneColor { get; internal set; }

    public void Set(string userID, string netSessionID)
    {
        UserID = userID;
        NetSessionID = netSessionID;
        IsReady = false;
    }

    public void SetReady()
    {
        IsReady = !IsReady;
    }
    public void SetReady(bool isReady)
    {
        IsReady = isReady;
    }

    public bool GetIsReady()
    {
        return IsReady;
    }

}