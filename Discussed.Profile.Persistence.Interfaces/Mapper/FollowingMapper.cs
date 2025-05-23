using System.Diagnostics;
using Discussed.Profile.Persistence.Interfaces.Contracts;
using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public partial class Mapper
{
    public async Task<IEnumerable<Following>> MapFollowingAsync(MySqlDataReader reader,
        CancellationToken cancellationToken)
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

    public async Task<IEnumerable<Follower>> MapFollowersAsync(MySqlDataReader reader,
        IEnumerable<string> fields,
        CancellationToken cancellationToken)
    {
        var result = new List<Follower>();
        var requestedFields = new HashSet<string>(fields, StringComparer.OrdinalIgnoreCase);

        while (await reader.ReadAsync(cancellationToken))
        {
            var mapped = new Follower();

            //Im aware this is called over and over but in most cases
            //we will not go over 2k rows so readability took priority
            if (requestedFields.Contains("userGuid"))
            {
                mapped.Id = reader.GetGuid("userGuid");
            }

            if (requestedFields.Contains("username"))
            {
                mapped.Username = reader.GetString("username");
            }

            result.Add(mapped);
        }

        return result;
    }
}