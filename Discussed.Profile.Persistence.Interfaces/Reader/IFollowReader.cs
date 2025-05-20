using Discussed.Profile.Persistence.Interfaces.Contracts;

namespace Discussed.Profile.Persistence.Interfaces.Reader;

public interface IFollowReader
{
    Task<(IEnumerable<Following>, int Total)> GetPageAsync(Guid userId, PagedOptions filter,
        CancellationToken cancellationToken);
}