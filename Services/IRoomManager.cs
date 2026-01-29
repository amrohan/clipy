
namespace clipy.Services;

public interface IRoomManager
{
    bool Exists(string roomCode);
    void Touch(string roomCode);
    IEnumerable<string> GetExpiredRooms(TimeSpan expiry);
    void Remove(string roomCode);
}