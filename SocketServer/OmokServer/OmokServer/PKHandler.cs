using System;


namespace OmokServer;

public class PKHandler
{
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;

    protected UserManager _userMgr = null;
    public RoomManager _roomMgr = null;

    public void Init(UserManager userMgr, RoomManager roomMgr)
    {
        this._userMgr = userMgr;
        this._roomMgr = roomMgr;
    }


}
