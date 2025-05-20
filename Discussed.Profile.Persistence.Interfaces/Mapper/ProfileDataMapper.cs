using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public partial class Mapper : IMapper
{
    public async Task<Contracts.Profile?> MapProfileAsync(MySqlDataReader reader, CancellationToken cancellationToken)
    {
        if (await reader.ReadAsync(cancellationToken))
        {
            return new Contracts.Profile
            {
                UserId = reader.GetGuid("userId"),
                FollowerCount = reader.GetInt32("followerCount"),
                FollowingCount = reader.GetInt32("followingCount"),
                Private = reader.GetBoolean("private")
            };
        }
        return null;
    }
}