public class RoomState
{
    public DateTime LastActivityUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastMessageUtc { get; set; } = DateTime.MinValue;
    public bool IsTyping { get; set; }
    public int ConnectionCount;
}