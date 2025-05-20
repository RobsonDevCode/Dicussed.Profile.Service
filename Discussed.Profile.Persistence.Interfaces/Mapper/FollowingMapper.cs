using Discussed.Profile.Persistence.Interfaces.Contracts;
using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public partial class Mapper
{
    public async Task<IEnumerable<Following>> MapFollowingAsync(MySqlDataReader reader, CancellationToken cancellationToken)
    {
        var result = new List<Following>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var mapped = new Following
            {
                UserId = reader.GetGuid("userGuid"),
                Username = reader.GetString("username"),
                FollowingId = reader.GetGuid("userFollowing"),
                Id = reader.GetGuid("id")
            };

            result.Add(mapped);
        }

        return result;
    }
}