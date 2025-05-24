using Discussed.Profile.Persistence.Interfaces.Contracts;
using Discussed.Profile.Persistence.Interfaces.Factories;
using Discussed.Profile.Persistence.Interfaces.Mapper;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Discussed.Profile.Persistence.MySql.Reader;

public sealed class ProfileReader : IProfileReader
{
    private readonly IMySqlConnectionFactory _mySqlConnectionFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfileReader> _logger;

    private static readonly HashSet<string> _validProfileColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "userId", "followerCount", "followingCount", "private", "bio"
    };

    public ProfileReader(IMySqlConnectionFactory mySqlConnectionFactory, IMapper mapper,
        ILogger<ProfileReader> logger)
    {
        _mySqlConnectionFactory = mySqlConnectionFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Interfaces.Contracts.Profile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM profile WHERE UserId = @userId LIMIT 1";
        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;

        var reader = await command.ExecuteReaderAsync(cancellationToken);
        var result = await _mapper.MapProfileAsync(reader, cancellationToken);
        return result;
    }

    public async Task<ProfileV2> GetByIdAsync(Guid userId, IEnumerable<string> fields,
        CancellationToken cancellationToken)
    {
        var sqlFields = string.Join(", ", fields.Where(x => _validProfileColumns.Contains(x))).TrimEnd(',');

        var sql = $"SELECT {sqlFields} FROM profile WHERE UserId = @userId LIMIT 1";

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;

        var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await _mapper.MapProfileAsync(reader, fields, cancellationToken);
    }

    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(id) FROM profile WHERE UserId = @userId LIMIT 1";
        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();

        await connection.OpenAsync(cancellationToken);
        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;
        var count = await command.ExecuteScalarAsync(cancellationToken);

        if (count is null)
        {
            _logger.LogWarning("count returned null when checking user exists.");
            return false;
        }

        return (int)count > 0;
    }
}