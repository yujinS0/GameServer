using MemoryPack;
using SqlKata.Execution;
using SuperSocket.SocketBase.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer;

internal class PKHMYSQL : PKHandler
{
    private QueryFactory _queryFactory;
    private readonly SuperSocket.SocketBase.Logging.ILog _logger;

    public PKHMYSQL(ILog logger) : base(logger)
    {
        this._logger = logger;
    }

    public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo, QueryFactory>> packetHandlerMap)
    {
        // Register all handlers with both MemoryPackBinaryRequestInfo and QueryFactory
        packetHandlerMap.Add((int)PACKETID.ReqInUserWin, (info, factory) => RequestInUserWin(factory, info));
        packetHandlerMap.Add((int)PACKETID.ReqInUserLose, (info, factory) => RequestInUserLose(factory, info));
        packetHandlerMap.Add((int)PACKETID.ReqInUserDraw, (info, factory) => RequestInUserDraw(factory, info));
        // Add other necessary handlers here
    }

    private void RequestInUserWin(QueryFactory _queryFactory, MemoryPackBinaryRequestInfo info)
    {
        var winPacket = MemoryPackSerializer.Deserialize<PKTReqInWin>(info.Data);
        string winnerId = winPacket.WinUserID;

        _logger.Debug($"Processing win for user ID: {winnerId}");

        var result = _queryFactory.Query("UserGameData")
            .Where("Email", "=", winnerId)
            .Increment("Win", 1);

        // Check the result and log accordingly
        if (result > 0)
        {
            
            _logger.Info($"Successfully updated win count for user ID: {winnerId}");
        }
        else
        {
            _logger.Error($"Failed to update win count for user ID: {winnerId}");
        }
    }
    private void RequestInUserLose(QueryFactory _queryFactory, MemoryPackBinaryRequestInfo info)
    {
        var losePacket = MemoryPackSerializer.Deserialize<PKTReqInLose>(info.Data);
        string loserId = losePacket.LoseUserID;

        _logger.Debug($"Processing win for user ID: {loserId}");

        var result = _queryFactory.Query("UserGameData")
            .Where("Email", "=", loserId)
            .Increment("Lose", 1);

        // Check the result and log accordingly
        if (result > 0)
        {
            _logger.Info($"Successfully updated lose count for user ID: {loserId}");
        }
        else
        {
            _logger.Error($"Failed to update lose count for user ID: {loserId}");
        }
    }

    private void RequestInUserDraw(QueryFactory _queryFactory, MemoryPackBinaryRequestInfo info)
    {
        var Packet = MemoryPackSerializer.Deserialize<PKTReqInDraw>(info.Data);
        string userId = Packet.UserID;

        _logger.Debug($"Processing win for user ID: {userId}");

        var result = _queryFactory.Query("UserGameData")
            .Where("Email", "=", userId)
            .Increment("Draw", 1);

        // Check the result and log accordingly
        if (result > 0)
        {
            _logger.Info($"Successfully updated draw count for user ID: {userId}");
        }
        else
        {
            _logger.Error($"Failed to update draw count for user ID: {userId}");
        }
    }

    private void RequestInMysql(QueryFactory _queryFactory, MemoryPackBinaryRequestInfo requestData)
    {
        _logger.Debug("RequestInMysql");
    }

}
