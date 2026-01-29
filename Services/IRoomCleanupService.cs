
namespace clipy.Services;

public interface IRoomCleanupService
{
    Task CheckAndCleanupAsync(CancellationToken cancellationToken);
}