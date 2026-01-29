using clipy.Services;
using Microsoft.AspNetCore.SignalR;

namespace clipy.Hubs;

public class RoomHub(IRoomManager rooms) : Hub
{
    public async Task JoinRoom(string roomCode)
    {
        rooms.Touch(roomCode);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
    }

    public bool RoomExists(string roomCode)
    {
        return rooms.Exists(roomCode);
    }

    public async Task SendMessage(string roomCode, string message)
    {
        rooms.Touch(roomCode);
        await Clients.Group(roomCode)
            .SendAsync("ReceiveMessage", message);
    }
}