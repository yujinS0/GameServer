using CloudStructures;
using CloudStructures.Structures;
using MemoryPack;
using SuperSocket.SocketBase.Logging;
using ServerClientCommon;

namespace OmokServer
{
    internal class PKHRedis : PKHandler
    {
        private readonly ILog _logger;

        public PKHRedis(ILog logger) : base(logger)
        {
            _logger = logger;
        }
        public void ClearRoomInfoList(RedisConnection redisConn)
        {
            var redisList = new RedisList<RoomInfo>(redisConn, "RoomInfoList", null);
            redisList.DeleteAsync().Wait();
            _logger.Info("Cleared existing RoomInfoList in Redis.");
        }

        public void RegistPacketHandler(Dictionary<int, Action<MemoryPackBinaryRequestInfo, RedisConnection>> packetHandlerMap)
        {
            packetHandlerMap.Add((int)PACKETID.ReqInRedisInsertRoomInfo, RequestInRedisInsertRoomInfo);
            packetHandlerMap.Add((int)PACKETID.ReqInRedisDeleteRoomInfo, RequestInRedisDeleteRoomInfo);
        }

        private void RequestInRedisInsertRoomInfo(MemoryPackBinaryRequestInfo requestData, RedisConnection redisConn)
        {
            var insertRoomInfo = MemoryPackSerializer.Deserialize<PKTReqInRedisInsertRoomInfo>(requestData.Body);
            _logger.Debug("RequestInRedisInsertRoomInfo");

            InsertRoomInfo(redisConn, insertRoomInfo.roomInfo);
        }

        private void RequestInRedisDeleteRoomInfo(MemoryPackBinaryRequestInfo requestData, RedisConnection redisConn)
        {
            var deleteRoomInfo = MemoryPackSerializer.Deserialize<PKTReqInRedisDeleteRoomInfo>(requestData.Body);
            _logger.Debug("RequestInRedisDeleteRoomInfo");

            DeleteRoomInfo(redisConn, deleteRoomInfo.RoomNumber);
        }

        private void InsertRoomInfo(RedisConnection redisConn, RoomInfo roomInfo)
        {
            var redisList = new RedisList<RoomInfo>(redisConn, "RoomInfoList", null);
            redisList.RightPushAsync(roomInfo).Wait();
            _logger.Info($"Inserted RoomInfo {roomInfo.RoomNumber} to Redis.");
        }

        private void DeleteRoomInfo(RedisConnection redisConn, int roomNumber)
        {
            var redisList = new RedisList<RoomInfo>(redisConn, "RoomInfoList", null);
            var roomInfoList = redisList.RangeAsync(0, -1).Result;
            var roomInfo = roomInfoList.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (roomInfo != null)
            {
                redisList.RemoveAsync(roomInfo).Wait();
                _logger.Info($"Deleted RoomInfo {roomNumber} from Redis.");
            }
        }
    }
}