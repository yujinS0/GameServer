using System;


namespace OmokServer;

public class PKHandler
{
    public static Func<string, byte[], bool> NetSendFunc;
    public static Action<MemoryPackBinaryRequestInfo> DistributeInnerPacket;

    protected UserManager _userMgr = null;


    public void Init(UserManager userMgr)
    {
        this._userMgr = userMgr;
    }           
            
    
}
