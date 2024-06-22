﻿using SuperSocket.SocketBase.Logging;

namespace OmokServer;
public class PKHandler
{
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;

    private readonly SuperSocket.SocketBase.Logging.ILog _logger;


    protected UserManager _userMgr = null;
    public RoomManager _roomMgr = null;

    public PKHandler(ILog logger)
    {
        this._logger = logger;
    }

    public void Init(UserManager userMgr, RoomManager roomMgr)
    {
        this._userMgr = userMgr;
        this._roomMgr = roomMgr;
    }
}
