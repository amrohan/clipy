using System.Collections.Concurrent;

namespace clipy.Services;

public class RoomManager : IRoomManager
{
    private readonly ConcurrentDictionary<string, RoomState> _rooms = new();

    public bool Exists(string roomCode)
        => _rooms.ContainsKey(roomCode);

    public void Touch(string roomCode)
    {
        _rooms.AddOrUpdate(
            roomCode,
            _ => new RoomState(),
            (_, room) =>
            {
                room.LastActivityUtc = DateTime.UtcNow;
                return room;
            });
    }

    public bool CanSendMessage(string roomCode, TimeSpan throttle)
    {
        var now = DateTime.UtcNow;

        var room = _rooms.GetOrAdd(roomCode, _ => new RoomState());

        lock (room) // per-room lock (cheap & safe)
        {
            if (now - room.LastMessageUtc < throttle)
                return false;

            room.LastMessageUtc = now;
            room.LastActivityUtc = now;
            return true;
        }
    }

    public IEnumerable<string> GetExpiredRooms(TimeSpan expiry)
    {
        return _rooms
            .Where(r => DateTime.UtcNow - r.Value.LastActivityUtc > expiry)
            .Select(r => r.Key);
    }

    public void Remove(string roomCode)
    {
        _rooms.TryRemove(roomCode, out _);
    }
}
