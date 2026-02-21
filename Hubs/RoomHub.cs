using clipy.Services;
using Microsoft.AspNetCore.SignalR;

namespace clipy.Hubs;

public class RoomHub(IRoomManager rooms) : Hub
{
    private static readonly TimeSpan MessageThrottle =
        TimeSpan.FromMilliseconds(200);

    public async Task JoinRoom(string roomCode)
    {
        rooms.Join(Context.ConnectionId, roomCode);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
    }

    public bool RoomExists(string roomCode)
    {
        return rooms.Exists(roomCode);
    }

    public async Task SendMessage(string roomCode, string message)
    {
        if (!rooms.CanSendMessage(roomCode, MessageThrottle))
            return;

        await Clients.OthersInGroup(roomCode)
            .SendAsync("ReceiveMessage", message);
    }

    /* ---------------------------
       Typing indicator
    ---------------------------- */
    public async Task Typing(string roomCode)
    {
        rooms.Touch(roomCode);

        await Clients.OthersInGroup(roomCode)
            .SendAsync("UserTyping");
    }

    public async Task StopTyping(string roomCode)
    {
        rooms.Touch(roomCode);

        await Clients.OthersInGroup(roomCode)
            .SendAsync("UserStoppedTyping");
    }

    public async Task CursorMove(string roomCode, int position)
    {
        rooms.Touch(roomCode);

        await Clients.OthersInGroup(roomCode)
            .SendAsync("ReceiveCursor", position);
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        rooms.Leave(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}