using Discussed.Profile.Persistence.Interfaces.Contracts;

namespace Discussed.Profile.Persistence.Interfaces.Reader;

public interface IProfileReader
{
    Task<Contracts.Profile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<ProfileV2> GetByIdAsync(Guid userId, IEnumerable<string> fields, CancellationToken cancellationToken);
}