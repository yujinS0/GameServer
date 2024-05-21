using MatchServer.Model.DTO;

namespace MatchServer.Repository;

public interface IMemoryDb : IDisposable
{
    Task<RoomInfo> PopRoomInfoAsync();
}
