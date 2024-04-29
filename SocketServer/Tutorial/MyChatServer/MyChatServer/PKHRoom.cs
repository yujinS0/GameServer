using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessagePack;

using CSBaseLib;


namespace ChatServer
{
    public class PKHRoom : PKHandler // 방과 관련된 로직 처리
                                                // 로직 처리 부분은 객체 만들어서 진행하고 있다~(이해무리없)
    {
        List<Room> RoomList = null;
        int StartRoomNumber;
        
        public void SetRooomList(List<Room> roomList)
        {
            RoomList = roomList;
            StartRoomNumber = roomList[0].Number;
        }

        public void RegistPacketHandler(Dictionary<int, Action<ServerPacketData>> packetHandlerMap)
        {
            packetHandlerMap.Add((int)PACKETID.REQ_ROOM_ENTER, RequestRoomEnter); // 방 입장
            packetHandlerMap.Add((int)PACKETID.REQ_ROOM_LEAVE, RequestLeave); // 방 퇴장 
            packetHandlerMap.Add((int)PACKETID.NTF_IN_ROOM_LEAVE, NotifyLeaveInternal); // 방을 나가지 않은 상태에서 접속 끊었을 때 -
                // 만약에 이 부분을 그냥 MainServer의 Onclosed로 해버리면, ?
                // MainServer의 Onclosed에서 PKHRoom의 함수들에 그냥 접근할 수는 없다. 
                // 룸 객체는 패킷 프로세서에서만 접근해야 한다!! (저쪽 스레드 에서만!)
                // 따라서 이때 일종의 내부 패킷을 만들어서(ex.NTF_IN_ROOM_LEAVE) 패킷으로 밀어넣고..!
                // => 룸의 패킷처리 스레드에서 방에서 나가는 부분을 처리하고 있다
                // 이러면 스레드 세이프 하게 처리할 수 있다!
            packetHandlerMap.Add((int)PACKETID.REQ_ROOM_CHAT, RequestChat);
        }


        Room GetRoom(int roomNumber)
        {
            var index = roomNumber - StartRoomNumber;

            if( index < 0 || index >= RoomList.Count())
            {
                return null;
            }

            return RoomList[index];
        }
                
        (bool, Room, RoomUser) CheckRoomAndRoomUser(string userNetSessionID)
        {
            var user = UserMgr.GetUser(userNetSessionID);
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



        public void RequestRoomEnter(ServerPacketData packetData)
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("RequestRoomEnter");

            try
            {
                var user = UserMgr.GetUser(sessionID);

                if (user == null || user.IsConfirm(sessionID) == false) // 방에 들어갔는데 또 방에 들어갔는지
                {
                    ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_USER, sessionID);
                    return;
                }

                if (user.IsStateRoom())
                {
                    ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_STATE, sessionID);
                    return;
                }

                var reqData = MessagePackSerializer.Deserialize<PKTReqRoomEnter>(packetData.BodyData);
                
                var room = GetRoom(reqData.RoomNumber);

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


                user.EnteredRoom(reqData.RoomNumber);

                room.NotifyPacketUserList(sessionID); // 방에 있는 유저에게 알려주기
                room.NofifyPacketNewUser(sessionID, user.ID());

                ResponseEnterRoomToClient(ERROR_CODE.NONE, sessionID); // 알려주기!

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

            var bodyData = MessagePackSerializer.Serialize(resRoomEnter);
            var sendData = PacketToBytes.Make(PACKETID.RES_ROOM_ENTER, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }

        public void RequestLeave(ServerPacketData packetData)
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("로그인 요청 받음");

            try
            {
                var user = UserMgr.GetUser(sessionID);
                if(user == null)
                {
                    return;
                }

                if(LeaveRoomUser(sessionID, user.RoomNumber) == false)
                {
                    return;
                }

                user.LeaveRoom();

                ResponseLeaveRoomToClient(sessionID); // 알려주기!

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

            var bodyData = MessagePackSerializer.Serialize(resRoomLeave);
            var sendData = PacketToBytes.Make(PACKETID.RES_ROOM_LEAVE, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }

        public void NotifyLeaveInternal(ServerPacketData packetData) // 방을 나가지 않은 상태에서 접속 끊었을 때
        {
            // 만약에 이 부분을 그냥 MainServer의 Onclosed로 해버리면, ?
            // MainServer의 Onclosed에서 PKHRoom의 함수들에 그냥 접근할 수는 없다. 
            // 룸 객체는 패킷 프로세서에서만 접근해야 한다!! (저쪽 스레드 에서만!)
            // 따라서 이때 하나의 내부 패킷을 만들어서 -
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug($"NotifyLeaveInternal. SessionID: {sessionID}");

            var reqData = MessagePackSerializer.Deserialize<PKTInternalNtfRoomLeave>(packetData.BodyData);            
            LeaveRoomUser(sessionID, reqData.RoomNumber);
        }
                
        public void RequestChat(ServerPacketData packetData) // 채팅 부분 - Broadcast
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


                var reqData = MessagePackSerializer.Deserialize<PKTReqRoomChat>(packetData.BodyData);

                var notifyPacket = new PKTNtfRoomChat()
                {
                    UserID = roomObject.Item3.UserID,
                    ChatMessage = reqData.ChatMessage
                };

                var Body = MessagePackSerializer.Serialize(notifyPacket);
                var sendData = PacketToBytes.Make(PACKETID.NTF_ROOM_CHAT, Body);

                roomObject.Item2.Broadcast("", sendData); // Broadcast

                MainServer.MainLogger.Debug("Room RequestChat - Success");
            }
            catch (Exception ex)
            {
                MainServer.MainLogger.Error(ex.ToString());
            }
        }
       

        

    }
}
