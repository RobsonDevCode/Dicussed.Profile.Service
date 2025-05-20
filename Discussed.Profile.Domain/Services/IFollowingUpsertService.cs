using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Services;

public interface IFollowingUpsertService
{
    Task FollowById(FollowRequestModel request, CancellationToken cancellationToken);
    Task AcceptRequestAsync(FollowRequestModel request, CancellationToken cancellationToken);
}