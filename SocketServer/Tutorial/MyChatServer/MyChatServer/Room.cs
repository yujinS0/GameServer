using CSBaseLib;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Room
    {
        public int Index { get; private set; }
        public int Number { get; private set; }

        int MaxUserCount = 0;

        List<RoomUser> UserList = new List<RoomUser>();

        public static Func<string, byte[], bool> NetSendFunc; // 여기도 주목! static <- 모든 룸 객체가 공통 사용
        // 이 "NetSendFunc" Func 만든 이유?!
        // 네트워크 샌드 하려면, 슈퍼소켓을 호출해야 함.
        // 그런데 여기는 슈퍼소켓 관련 객체를 가지고 있지 않음 (MainServer에 있)
        // 근데 Broadcast하려면 Mainserver 객체를 룸에서 가지고 있어야 한다~
        // 근데 메인서버가 쟤를 가지면 가지지 룸이 얘를 가지면 상호 참조가 되어버림
        // 그래서 슈퍼소켓의 샌드 함수 부분만 사용하려고 NetSendFunc 함수로 참조를 하고 있다.
        // !!!! 그래서 NetSendFunc 호출하면 결론적으로 MainServer의 SendData가 호출되는 것 !!!!


        public void Init(int index, int number, int maxUserCount)
        {
            Index = index;
            Number = number;
            MaxUserCount = maxUserCount;
        }

        public bool AddUser(string userID, string netSessionID)
        {
            if(GetUser(userID) != null)
            {
                return false;
            }

            var roomUser = new RoomUser();
            roomUser.Set(userID, netSessionID);
            UserList.Add(roomUser);

            return true;
        }

        public void RemoveUser(string netSessionID)
        {
            var index = UserList.FindIndex(x => x.NetSessionID == netSessionID);
            UserList.RemoveAt(index);
        }

        public bool RemoveUser(RoomUser user)
        {
            return UserList.Remove(user);
        }

        public RoomUser GetUser(string userID)
        {
            return UserList.Find(x => x.UserID == userID);
        }

        public RoomUser GetUserByNetSessionId(string netSessionID)
        {
            return UserList.Find(x => x.NetSessionID == netSessionID);
        }

        public int CurrentUserCount()
        {
            return UserList.Count();
        }

        public void NotifyPacketUserList(string userNetSessionID)
        {
            var packet = new CSBaseLib.PKTNtfRoomUserList();
            foreach (var user in UserList)
            {
                packet.UserIDList.Add(user.UserID);
            }

            var bodyData = MessagePackSerializer.Serialize(packet);
            var sendPacket = PacketToBytes.Make(PACKETID.NTF_ROOM_USER_LIST, bodyData);

            NetSendFunc(userNetSessionID, sendPacket);
        }

        public void NofifyPacketNewUser(string newUserNetSessionID, string newUserID)
        {
            var packet = new PKTNtfRoomNewUser();
            packet.UserID = newUserID;
            
            var bodyData = MessagePackSerializer.Serialize(packet);
            var sendPacket = PacketToBytes.Make(PACKETID.NTF_ROOM_NEW_USER, bodyData);

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
            
            var bodyData = MessagePackSerializer.Serialize(packet);
            var sendPacket = PacketToBytes.Make(PACKETID.NTF_ROOM_LEAVE_USER, bodyData);

            Broadcast("", sendPacket);
        }

        public void Broadcast(string excludeNetSessionID, byte[] sendPacket) // 채팅 처리 Broadcast
        {
            foreach(var user in UserList)
            {
                if(user.NetSessionID == excludeNetSessionID)
                {
                    continue;
                }

                NetSendFunc(user.NetSessionID, sendPacket);
            }
        }
    }


    public class RoomUser
    {
        public string UserID { get; private set; }
        public string NetSessionID { get; private set; }

        public void Set(string userID, string netSessionID)
        {
            UserID = userID;
            NetSessionID = netSessionID;
        }
    }
}
