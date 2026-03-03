namespace clipy.Services;

public interface IRoomManager
{
    bool Exists(string roomCode);
    void Touch(string roomCode);
    bool CanSendMessage(string roomCode, TimeSpan throttle);
    IEnumerable<string> GetExpiredRooms(TimeSpan expiry);
    void Remove(string roomCode);
    void Join(string connectionId, string roomCode);
    void Leave(string connectionId);
}