using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Services;

public interface IFollowingRemovalService
{
    Task DeclineFollowRequest(DeclineRequestModel request, CancellationToken cancellationToken);
}