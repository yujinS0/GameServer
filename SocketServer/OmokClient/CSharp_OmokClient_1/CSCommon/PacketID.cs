using System;
using System.Collections.Generic;
using System.Text;

namespace CSCommon
{
    public enum PACKETID : int
    {
        REQ_RES_TEST_ECHO = 101,


        // 클라이언트
        CS_BEGIN = 1001,

        REQ_LOGIN = 1002,
        RES_LOGIN = 1003,
        NTF_MUST_CLOSE = 1005,

        REQ_ROOM_ENTER = 1015,
        RES_ROOM_ENTER = 1016,
        NTF_ROOM_USER_LIST = 1017,
        NTF_ROOM_NEW_USER = 1018,

        REQ_ROOM_LEAVE = 1021,
        RES_ROOM_LEAVE = 1022,
        NTF_ROOM_LEAVE_USER = 1023,

        REQ_ROOM_CHAT = 1026,
        NTF_ROOM_CHAT = 1027,


        REQ_ROOM_DEV_ALL_ROOM_START_GAME = 1091,
        RES_ROOM_DEV_ALL_ROOM_START_GAME = 1092,

        REQ_ROOM_DEV_ALL_ROOM_END_GAME = 1093,
        RES_ROOM_DEV_ALL_ROOM_END_GAME = 1094,

        CS_END = 1100,

        // 오목 게임 로직 관련
        REQ_GAME_START = 2001,
        NTF_GAME_START = 2002,

        REQ_PLACE_STONE = 2010,
        NTF_PLACE_STONE = 2011,

        NTF_GAME_END = 2020,


        ReqReadyOmok = 1031,
        ResReadyOmok = 1032,
        NtfReadyOmok = 1033,

        NtfStartOmok = 1034,

        ReqPutMok = 1035,
        ResPutMok = 1036,
        NTFPutMok = 1037,

        NTFEndOmok = 1038,


        END = 1100,


        // 시스템, 서버 - 서버
        SS_START = 8001,

        NTF_IN_CONNECT_CLIENT = 8011,
        NTF_IN_DISCONNECT_CLIENT = 8012,

        REQ_SS_SERVERINFO = 8021,
        RES_SS_SERVERINFO = 8023,

        REQ_IN_ROOM_ENTER = 8031,
        RES_IN_ROOM_ENTER = 8032,

        NTF_IN_ROOM_LEAVE = 8036,


        // DB 8101 ~ 9000
        REQ_DB_LOGIN = 8101,
        RES_DB_LOGIN = 8102,


    //public const UInt16 BEGIN = 1001;

    //public const UInt16 ReqLogin = 1002;
    //public const UInt16 ResLogin = 1003;

    //public const UInt16 NtfMustClose = 1005;

    //public const UInt16 ReqRoomEnter = 1015;
    //public const UInt16 ResRoomEnter = 1016;
    //public const UInt16 NtfRoomUserList = 1017;
    //public const UInt16 NtfRoomNewUser = 1018;

    //public const UInt16 ReqRoomLeave = 1021;
    //public const UInt16 ResRoomLeave = 1022;
    //public const UInt16 NtfRoomLeaveUser = 1023;

    //public const UInt16 ReqRoomChat = 1026;
    //public const UInt16 ResRoomChat = 1027;
    //public const UInt16 NtfRoomChat = 1028;

    //public const UInt16 ReqReadyOmok = 1031;
    //public const UInt16 ResReadyOmok = 1032;
    //public const UInt16 NtfReadyOmok = 1033;

    //public const UInt16 NtfStartOmok = 1034;

    //public const UInt16 ReqPutMok = 1035;
    //public const UInt16 ResPutMok = 1036;
    //public const UInt16 NTFPutMok = 1037;

    //public const UInt16 NTFEndOmok = 1038;


    //public const UInt16 END = 1100;
    }
}
