using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer;

internal class PKHRedis : PKHandler
{
    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> packetHandlerMap)
    {
        packetHandlerMap.Add((int)PACKETID.ReqInRedis, RequestInRedis);

    }

    private void RequestInRedis(MemoryPackBinaryRequestInfo requestData)
    {
        MainServer.MainLogger.Debug("RequestIn Redis");
    }

}
