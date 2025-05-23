using Discussed.Profile.Persistence.Interfaces.Contracts;
using Discussed.Profile.Persistence.Interfaces.Factories;
using Discussed.Profile.Persistence.Interfaces.Mapper;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Discussed.Profile.Persistence.MySql.Reader;

public sealed class FollowReader : IFollowReader
{
    private readonly IMySqlConnectionFactory _mySqlConnectionFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<FollowReader> _logger;

    private static readonly HashSet<string> _allowedFollowingFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "userGuid", "username", "userFollowing", "id"
    };

    public FollowReader(IMySqlConnectionFactory mySqlConnectionFactory,
        IMapper mapper,
        ILogger<FollowReader> logger)
    {
        _mySqlConnectionFactory = mySqlConnectionFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<Following>, int Total)> GetFollowingPageAsync(Guid userId, PagedOptions filter,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT * FROM following
                           WHERE userGuid = @userId LIMIT @pagesize OFFSET @skip;
                           """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;
        command.Parameters.Add("@skip", MySqlDbType.Int32).Value = filter.Skip;
        command.Parameters.Add("@pageSize", MySqlDbType.Int32).Value = filter.Take;

        var reader = await command.ExecuteReaderAsync(cancellationToken);

        var total = await GetTotalFollowingAsync(userId, cancellationToken);

        return (await _mapper.MapFollowingAsync(reader, cancellationToken), total);
    }

    public async Task<IEnumerable<Follower>> GetFollowerPageAsync(Guid userId, IEnumerable<string> fields,
        PagedOptions filter, CancellationToken cancellationToken)
    {
        //you cannot parameterize field names in SQL so we compare against a hashset of allowed columns to prevent sql injection
        var sqlFields = string.Join(", ", fields.Where(x => _allowedFollowingFields.Contains(x)))
            .TrimEnd(',');

        var sql = $"""
                    SELECT {sqlFields} FROM following
                    WHERE userFollowing = @userId LIMIT @pagesize OFFSET @skip;
                  """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new MySqlCommand(sql, connection);

        _logger.LogInformation("Getting fields {fields} for following page.", sqlFields);

        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;
        command.Parameters.Add("@skip", MySqlDbType.Int32).Value = filter.Skip;
        command.Parameters.Add("@pageSize", MySqlDbType.Int32).Value = filter.Take;

        var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await _mapper.MapFollowersAsync(reader, fields, cancellationToken);
    }

    public async Task<int> GetTotalFollowingAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = """
                            SELECT COUNT(userGuid) FROM following WHERE userGuid = @userId;
                           """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();

        await connection.OpenAsync(cancellationToken);
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;

        var response = await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt32(response);
    }
    public async Task<int> GetTotalFollowersAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = """
                            SELECT COUNT(userGuid) FROM following WHERE userFollowing = @userId;
                           """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();

        await connection.OpenAsync(cancellationToken);
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;

        var response = await command.ExecuteScalarAsync(cancellationToken);

        return Convert.ToInt32(response);
    }
}