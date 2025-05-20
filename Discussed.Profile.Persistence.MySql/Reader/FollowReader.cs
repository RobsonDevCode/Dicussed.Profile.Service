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

    public FollowReader(IMySqlConnectionFactory mySqlConnectionFactory,
        IMapper mapper,
        ILogger<FollowReader> logger)
    {
        _mySqlConnectionFactory = mySqlConnectionFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<Following>, int Total)> GetPageAsync(Guid userId, PagedOptions filter,
        CancellationToken cancellationToken)
    {
        try
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

            var total = await GetTotalAsync(userId, cancellationToken);
            
            return (await _mapper.MapFollowingAsync(reader, cancellationToken), total);
        }
        catch (Exception ex)
        {
            _logger.LogError("error getting page from database: {error}, {errorDetails}", ex, ex.Message);
            throw;
        }
    }

    public async Task<int> GetTotalAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogInformation("error getting total count: {error}, {errorDetails}", ex, ex.Message);
            throw;
        }
    }
}