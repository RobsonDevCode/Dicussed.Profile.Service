using Discussed.Profile.Persistence.Interfaces.Contracts;
using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public interface IFollowingMapper
{
    Task<IEnumerable<Following>> MapFollowingAsync(MySqlDataReader reader, CancellationToken cancellationToken);
}