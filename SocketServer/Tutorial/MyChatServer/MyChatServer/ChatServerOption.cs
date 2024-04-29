using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatServerOption
    {
        [Option( "uniqueID", Required = true, HelpText = "Server UniqueID")]
        public int ChatServerUniqueID { get; set; }

        [Option("name", Required = true, HelpText = "Server Name")]
        public string Name { get; set; }

        //////// 슈퍼소켓라이브러리 사용을 위한 옵션 부분 /////////
        [Option("maxConnectionNumber", Required = true, HelpText = "MaxConnectionNumber")]
        public int MaxConnectionNumber { get; set; }

        [Option("port", Required = true, HelpText = "Port")]
        public int Port { get; set; }

        [Option("maxRequestLength", Required = true, HelpText = "maxRequestLength")]
        public int MaxRequestLength { get; set; }

        [Option("receiveBufferSize", Required = true, HelpText = "receiveBufferSize")]
        public int ReceiveBufferSize { get; set; }

        [Option("sendBufferSize", Required = true, HelpText = "sendBufferSize")]
        public int SendBufferSize { get; set; }
        ////////------------------------------------------/////// 

        // ------채팅서버에서 사용하는 옵션--------
        [Option("roomMaxCount", Required = true, HelpText = "Max Romm Count")]
        public int RoomMaxCount { get; set; } = 0; // 방 최대 몇개

        [Option("roomMaxUserCount", Required = true, HelpText = "RoomMaxUserCount")]
        public int RoomMaxUserCount { get; set; } = 0; // 방에 들어갈 수 있는 최대 유저수

        [Option("roomStartNumber", Required = true, HelpText = "RoomStartNumber")]
        public int RoomStartNumber { get; set; } = 0; // 방의 시작번호
        // 룸이 서로 겹치면 안된다
        // 채팅 서버 두개가 돌아가고 있는데, 룸 1번이 어느 서버로 들어가야하는지..?
        // 모르니깐 해당 부분 구분을 위해 (여러 채팅서버를 실행해도 각 룸 번호가 안겹치게)
    }
}
