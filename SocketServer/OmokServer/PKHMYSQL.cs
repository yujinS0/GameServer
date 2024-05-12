using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer;

internal class PKHMYSQL : PKHandler
{
    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo>> packetHandlerMap)
    {
        packetHandlerMap.Add((int)PACKETID.ReqInMysql, RequestInMysql);

    }

    private void RequestInMysql(MemoryPackBinaryRequestInfo requestData)
    {
        MainServer.MainLogger.Debug("RequestInMysql");
    }

}
