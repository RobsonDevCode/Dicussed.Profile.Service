using Discussed.Profile.Persistence.Interfaces.Contracts;

namespace Discussed.Profile.Persistence.Interfaces.Writer;

public interface IFollowWriter
{
  Task AddFollowRequest(Follow request, CancellationToken cancellationToken);
  Task FollowUser(Follow request, CancellationToken cancellationToken);
  
  Task RemoveFollowRequest(RemoveFollowRequest request, CancellationToken cancellationToken);
}