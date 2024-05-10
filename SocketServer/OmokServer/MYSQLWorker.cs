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

namespace OmokServer;

class MYSQLWorker
{
    bool _isThreadRunning = false;
    // 스레드 리스트
    List<System.Threading.Thread> _processThreadList = new List<System.Threading.Thread>();

    //public Func<string, byte[], bool> SendInnerPacketFunc;
    public Func<string, byte[], bool> NetSendFunc;
    BufferBlock<MemoryPackBinaryRequestInfo> _msgBuffer = new BufferBlock<MemoryPackBinaryRequestInfo>();

    Dictionary<int, Action<MemoryPackBinaryRequestInfo>> _packetHandlerMap = new Dictionary<int, Action<MemoryPackBinaryRequestInfo>>();

    string db_connection;

    PKHMYSQL _mysqlPacketHandler = new PKHMYSQL();

    UserManager _userMgr = new UserManager();
    RoomManager _roomMgr;

    public void CreateAndStart()
    {
        //IOptions<ConnectionStrings> connectionStrings; // TODO : Config에서 가져오기
        db_connection = "server=localhost;port=3306;user=root;password=000930;database=gamedb";

        RegistPacketHandler();

        _isThreadRunning = true;

        for (int i = 0; i < 4; i++) // 스레드 4개 만든다고 가정
        {
            var thread = new System.Threading.Thread(this.Process);
            _processThreadList.Add(thread);
            thread.Start();
        }
    }

    public void Destory()
    {
        MainServer.MainLogger.Info("MYSQLWorker::Destory - begin");

        _isThreadRunning = false;
        _msgBuffer.Complete();

        foreach (var thread in _processThreadList)
        {
            thread.Join();
        }

        MainServer.MainLogger.Info("MYSQLWorker::Destory - end");
    }

    public void InsertPacket(MemoryPackBinaryRequestInfo packet)
    {
        _msgBuffer.Post(packet);
    }

    void RegistPacketHandler()
    {
        // mysql 용 패킷 핸들러 등록 
        PKHandler.NetSendFunc = NetSendFunc;
        PKHandler.DistributeInnerPacket = InsertPacket;

        _mysqlPacketHandler.Init(_userMgr, _roomMgr);
        _mysqlPacketHandler.RegistPacketHandler(_packetHandlerMap);
    }

    void Process()
    {
        MySqlConnection _connection = new MySqlConnection(db_connection);
        _connection.Open();

        var compiler = new MySqlCompiler();
        QueryFactory _queryFactory = new QueryFactory(_connection, new MySqlCompiler());

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
