using Discussed.Profile.Persistence.Interfaces.Contracts;
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
                Private = reader.GetBoolean("private"),
                Username = null,
                Bio = null
            };
        }

        return null;
    }

    public async Task<ProfileV2?> MapProfileAsync(MySqlDataReader reader, IEnumerable<string> fields,
        CancellationToken cancellationToken)
    {
        var requestedFields = new HashSet<string>(fields);

        if (!await reader.ReadAsync(cancellationToken)) 
            return null;
    
        var result = new ProfileV2();

        // Define field mappings
        var fieldMappings = new Dictionary<string, Action>
        {
            ["userId"] = () => result.UserId = reader.GetGuid("userId"),
            ["username"] = () => result.Username = reader.GetString("username"),
            ["followerCount"] = () => result.FollowerCount = reader.GetInt32("followerCount"),
            ["followingCount"] = () => result.FollowingCount = reader.GetInt32("followingCount"),
            ["private"] = () => result.Private = reader.GetBoolean("private"),
            ["bio"] = () => result.Bio = reader.GetString("bio")
        };

        // Apply mappings for requested fields
        foreach (var field in requestedFields.Where(fieldMappings.ContainsKey))
        {
            fieldMappings[field]();
        }
        
        return result;
    }
}