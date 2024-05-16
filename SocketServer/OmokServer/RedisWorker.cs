using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using CloudStructures;
using CloudStructures.Structures;
using SuperSocket.SocketBase.Logging;

namespace OmokServer;

class RedisWorker
{
    bool _isThreadRunning = false;
    System.Threading.Thread _processThread = null;

    //public Func<string, byte[], bool> SendInnerPacketFunc;
    public Func<string, byte[], bool> NetSendFunc;
    BufferBlock<MemoryPackBinaryRequestInfo> _msgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();

    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _packetHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();

    string db_connection;

    PKHRedis _redisPacketHandler;

    UserManager _userMgr;
    RoomManager _roomMgr;
    private readonly SuperSocket.SocketBase.Logging.ILog _logger;

    public RedisWorker(ILog logger)
    {
        this._logger = logger;
        _redisPacketHandler = new PKHRedis(_logger);
        _userMgr = new UserManager(_logger);
        _roomMgr = new RoomManager(_logger);
    }

    public void CreateAndStart()
    {
        //IOptions<ConnectionStrings> connectionStrings; // TODO : Config에서 가져오기
        db_connection = "localhost:6389";

        RegistPacketHandler();

        _isThreadRunning = true;
        _processThread = new System.Threading.Thread(this.Process);
        _processThread.Start();
    }

    public void Destory()
    {
        MainServer.MainLogger.Info("RedisWorker::Destory - begin");

        _isThreadRunning = false;
        _msgBuffer.Complete();

        _processThread.Join();

        MainServer.MainLogger.Info("RedisWorker::Destory - end");
    }

    public void InsertPacket(MemoryPackBinaryRequestInfo packet)
    {
        _msgBuffer.Post(packet);
    }

    void RegistPacketHandler()
    {
        // redis 용 패킷 핸들러 등록 
        PKHandler.NetSendFunc = NetSendFunc;
        PKHandler.DistributeInnerPacket = InsertPacket;

        _redisPacketHandler.Init(_userMgr, _roomMgr);
        _redisPacketHandler.RegistPacketHandler(_packetHandlerMap);
    }

    void Process()
    {
        RedisConfig config = new RedisConfig("default", db_connection);
        RedisConnection _redisConn = new RedisConnection(config);

        try
        {
            while (_isThreadRunning)
            {
                var packet = _msgBuffer.Receive();
                if (packet == null)
                {
                    continue;
                }
                var header = new MemoryPackPacketHeadInfo();
                header.Read(packet.Data);

                if (_packetHandlerMap.ContainsKey(header.Id))
                {
                    _packetHandlerMap[header.Id](packet);
                }
            }
        }
        catch (Exception ex)
        {
            if (_isThreadRunning)
            {
                MainServer.MainLogger.Error(ex.ToString());
            }
        }

    }

}
