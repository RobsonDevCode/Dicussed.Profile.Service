using Discussed.Profile.Persistence.Interfaces.Contracts;
using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public interface IProfileDataMapper
{
    Task<Contracts.Profile?> MapProfileAsync(MySqlDataReader reader, CancellationToken cancellationToken);

    Task<ProfileV2?> MapProfileAsync(MySqlDataReader reader, IEnumerable<string> fields,
        CancellationToken cancellationToken);
}