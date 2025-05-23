using Discussed.Profile.Persistence.Interfaces.Contracts;
using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public interface IFollowingMapper
{
    Task<IEnumerable<Following>> MapFollowingAsync(MySqlDataReader reader, CancellationToken cancellationToken);

    Task<IEnumerable<Follower>> MapFollowersAsync(MySqlDataReader reader,
        IEnumerable<string> fields,
        CancellationToken cancellationToken);

}