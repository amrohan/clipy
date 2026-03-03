using clipy.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace clipy.Services;

public class RoomCleanupService(
    IRoomManager rooms,
    IHubContext<RoomHub> hub) : BackgroundService, IRoomCleanupService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAndCleanupAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    public async Task CheckAndCleanupAsync(CancellationToken cancellationToken)
    {
        foreach (var room in rooms.GetExpiredRooms(Expiry))
        {
            await hub.Clients.Group(room).SendAsync("RoomExpired", cancellationToken: cancellationToken);
            rooms.Remove(room);
        }
    }
}