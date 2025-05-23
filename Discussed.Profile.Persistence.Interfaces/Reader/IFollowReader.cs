using Discussed.Profile.Persistence.Interfaces.Contracts;

namespace Discussed.Profile.Persistence.Interfaces.Reader;

public interface IFollowReader
{
    Task<(IEnumerable<Following>, int Total)> GetFollowingPageAsync(Guid userId, PagedOptions filter,
        CancellationToken cancellationToken);

    Task<IEnumerable<Follower>> GetFollowerPageAsync(Guid userId, IEnumerable<string> fields,
        PagedOptions filter, CancellationToken cancellationToken);

    Task<int> GetTotalFollowersAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> GetTotalFollowingAsync(Guid userId, CancellationToken cancellationToken);
}