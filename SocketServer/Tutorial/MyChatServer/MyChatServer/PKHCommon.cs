﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessagePack;

using CSBaseLib;


namespace ChatServer
{
    public class PKHCommon : PKHandler // 공통으로 처리해야하는 부분 (로직) 요청 처리
    {
        public void RegistPacketHandler(Dictionary<int, Action<ServerPacketData>> packetHandlerMap)
        {            
            // 패킷ID와 핸들러 함수(델리게이트) 쌍을 Map에 추가
            packetHandlerMap.Add((int)PACKETID.NTF_IN_CONNECT_CLIENT, NotifyInConnectClient);
            packetHandlerMap.Add((int)PACKETID.NTF_IN_DISCONNECT_CLIENT, NotifyInDisConnectClient);

            packetHandlerMap.Add((int)PACKETID.REQ_LOGIN, RequestLogin);
                                                
        }

        public void NotifyInConnectClient(ServerPacketData requestData) // 새로운 연결 생길 때
        {
            MainServer.MainLogger.Debug($"Current Connected Session Count: {ServerNetwork.SessionCount}");
        }

        public void NotifyInDisConnectClient(ServerPacketData requestData) // 접속 세션의 연결 끊김
        {
            var sessionID = requestData.SessionID;
            var user = UserMgr.GetUser(sessionID);
            
            if (user != null)
            {
                var roomNum = user.RoomNumber;

                if (roomNum != PacketDef.INVALID_ROOM_NUMBER)
                {
                    var packet = new PKTInternalNtfRoomLeave()
                    {
                        RoomNumber = roomNum,
                        UserID = user.ID(),
                    };

                    var packetBodyData = MessagePackSerializer.Serialize(packet);
                    var internalPacket = new ServerPacketData();
                    internalPacket.Assign(sessionID, (Int16)PACKETID.NTF_IN_ROOM_LEAVE, packetBodyData); // 내부 패킷

                    ServerNetwork.Distribute(internalPacket);
                }

                UserMgr.RemoveUser(sessionID);
            }
                        
            MainServer.MainLogger.Debug($"Current Connected Session Count: {ServerNetwork.SessionCount}");
        }


        public void RequestLogin(ServerPacketData packetData) // 로그인 되었을 때
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("로그인 요청 받음");

            try
            {
                if(UserMgr.GetUser(sessionID) != null)
                {
                    ResponseLoginToClient(ERROR_CODE.LOGIN_ALREADY_WORKING, packetData.SessionID);
                    return;
                }
                                
                var reqData = MessagePackSerializer.Deserialize< PKTReqLogin>(packetData.BodyData);
                var errorCode = UserMgr.AddUser(reqData.UserID, sessionID);
                if (errorCode != ERROR_CODE.NONE)
                {
                    ResponseLoginToClient(errorCode, packetData.SessionID);

                    if (errorCode == ERROR_CODE.LOGIN_FULL_USER_COUNT)
                    {
                        NotifyMustCloseToClient(ERROR_CODE.LOGIN_FULL_USER_COUNT, packetData.SessionID);
                    }
                    
                    return;
                }

                ResponseLoginToClient(errorCode, packetData.SessionID);

                MainServer.MainLogger.Debug("로그인 요청 답변 보냄");

            }
            catch(Exception ex)
            {
                // 패킷 해제에 의해서 로그가 남지 않도록 로그 수준을 Debug로 한다.
                MainServer.MainLogger.Error(ex.ToString());
            }
        }
                
        public void ResponseLoginToClient(ERROR_CODE errorCode, string sessionID)
        {
            var resLogin = new PKTResLogin()
            {
                Result = (short)errorCode
            };

            var bodyData = MessagePackSerializer.Serialize(resLogin);
            var sendData = PacketToBytes.Make(PACKETID.RES_LOGIN, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }

        public void NotifyMustCloseToClient(ERROR_CODE errorCode, string sessionID)
        {
            var resLogin = new PKNtfMustClose()
            {
                Result = (short)errorCode
            };

            var bodyData = MessagePackSerializer.Serialize(resLogin);
            var sendData = PacketToBytes.Make(PACKETID.NTF_MUST_CLOSE, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }


        
                      
    }
}
