using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSBaseLib;
using MessagePack;

namespace ChatServer
{
    public class ServerPacketData
    {
        public Int16 PacketSize;
        public string SessionID; 
        public Int16 PacketID;        
        public SByte Type;
        public byte[] BodyData;
                
        // 내부 패킷 처리
        public void Assign(string sessionID, Int16 packetID, byte[] packetBodyData)
        {
            SessionID = sessionID;
            PacketID = packetID;
            
            if (packetBodyData.Length > 0)
            {
                BodyData = packetBodyData;
            }
        }
                
        // 내부 패킷처리 - 연결인지 아닌지 처리하는 거!!!
        public static ServerPacketData MakeNTFInConnectOrDisConnectClientPacket(bool isConnect, string sessionID)
        {
            var packet = new ServerPacketData();
            
            if (isConnect)
            {
                packet.PacketID = (Int32)PACKETID.NTF_IN_CONNECT_CLIENT; // 이 부분이 패킷 ID를 조정하는 부분.
            }
            else
            {
                packet.PacketID = (Int32)PACKETID.NTF_IN_DISCONNECT_CLIENT;
            }

            packet.SessionID = sessionID;
            return packet;
        }               
        
    }



    [MessagePackObject]
    public class PKTInternalReqRoomEnter
    {
        [Key(0)]
        public int RoomNumber;

        [Key(1)]
        public string UserID;        
    }

    [MessagePackObject]
    public class PKTInternalResRoomEnter
    {
        [Key(0)]
        public ERROR_CODE Result;

        [Key(1)]
        public int RoomNumber;

        [Key(2)]
        public string UserID;
    }


    [MessagePackObject]
    public class PKTInternalNtfRoomLeave
    {
        [Key(0)]
        public int RoomNumber;

        [Key(1)]
        public string UserID;
    }

}
